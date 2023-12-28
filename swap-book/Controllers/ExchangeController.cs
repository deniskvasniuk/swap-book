using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using swap_book.Models;
using swap_book.Services;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;


namespace swap_book.Controllers
{
	public class ExchangeController : Controller
	{
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMessageService _messageService;


        public ExchangeController(DatabaseContext context, UserManager<ApplicationUser> userManager, IMessageService messageService)
		{
			_context = context;
			_userManager = userManager;
			this._messageService = messageService;

		}
		public IActionResult Index()
		{
			return View(_context.Books);
		}

        [HttpGet]
        public JsonResult GetAllBooksForUser()
        {
            var userId = _userManager.GetUserId(User);

            var books = _context.Books

                .Where(b => b.OwnerId == userId)
                .ToList();

            var jsonBooks = books.Select(b => new {
                Id = b.BookId,
                Title = b.Name,
                Author = b.Author,
                ImageUrl = b.ImageUrl,
                Description = b.Description,
                Exchangeable = b.Exchangeable,
                OwnerId = b.OwnerId
            });

            return Json(jsonBooks);
        }

        [HttpPost]
        public async Task <IActionResult> Create(int exchangedBookId)
        {
            var userId = _userManager.GetUserId(User);
            var bookId = int.Parse(Request.Form["BookId"]);
			var bookOwnerId = _context.Books
				.Where(book => book.BookId == bookId)
				.Select(book => book.OwnerId)
				.FirstOrDefault();

			var exchangedBookOwnerId = _context.Books
				.Where(book => book.BookId == exchangedBookId)
				.Select(book => book.OwnerId)
				.FirstOrDefault();

			if (bookOwnerId == exchangedBookOwnerId)
			{
				TempData["AlertMessage"] = "Book owners cannot exchange books with themselves.";
				return Redirect(HttpContext.Request.Headers["Referer"].ToString());
			}
			var exchange = new Exchange
            {
                UserId = userId,
                BookId = bookId,
                ExchangedBookId = exchangedBookId,
                Status = Exchange.ExchangeStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.Exchanges.Add(exchange);
            await SendConfirmMessage(exchange);
            await _context.SaveChangesAsync();


			return Redirect(HttpContext.Request.Headers["Referer"].ToString());
		}
		public async Task SendConfirmMessage(Exchange exchange)
		{
			if (exchange.Status != Exchange.ExchangeStatus.Pending)
			{
				throw new ArgumentException("Exchange must be in Pending status.");
			}

			var sender = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == exchange.UserId);
            var owner = await _context.Books
                .Include(b => b.Owner) 
                .FirstOrDefaultAsync(b => b.BookId == exchange.ExchangedBookId);

            var recipient = owner.Owner;
            

            var message = new Message
			{
				SenderId = sender.Id,
				RecipientId = recipient.Id,
				Content = $"You have received a book exchange request from {sender.Name}.\n\nWould you like to accept the exchange?",
				SentDate = DateTime.UtcNow
			};

			await _messageService.SendMessage(sender, recipient, message);
		}

        [HttpPost]
        public async Task<IActionResult> ConfirmExchange(int exchangeId)
        {
            var exchange = await _context.Exchanges
                .FirstOrDefaultAsync(e => e.ExchangeId == exchangeId);

            if (exchange == null)
            {

                return NotFound();
            }

            exchange.Status = Exchange.ExchangeStatus.Accepted;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Exchange is successfully confirmed!";

            return Redirect(HttpContext.Request.Headers["Referer"].ToString()); 
        }
	}
}
