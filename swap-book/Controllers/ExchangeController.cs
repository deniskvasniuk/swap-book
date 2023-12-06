using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using swap_book.Models;

namespace swap_book.Controllers
{
	public class ExchangeController : Controller
	{
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExchangeController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
			_context=context;
            _userManager=userManager;
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
        public IActionResult Create(int exchangedBookId)
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
				return RedirectToAction("Index", "Home");
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
            _context.SaveChanges();

  
            return RedirectToAction("Index", "Home");
        }
    }
}
