using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Razor;
using swap_book.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using swap_book.Services;
using swap_book.Models;
using System.Web;


namespace swap_book.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

		private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

		   
		   var books = _context.Books
			   .Include(b => b.Owner)
			   .Include(b => b.Wishlists)
			   .ToList();

		   // Select the latest books
		   var latestBooks = books
			   .OrderByDescending(b => b.CreatedAt)
			   .Take(10).ToList();

		   var currentUser = await _userManager.GetUserAsync(User);

			// Prepare an instance of the ViewModel
			var viewModel = new IndexViewModel(books, latestBooks, currentUser);

		   return View(viewModel);
		}
        public IActionResult Contact()
        {

            return View("~/Views/Contact/Index.cshtml");
        }

        public IActionResult UserProfile()
        {

            return View("~/Views/User/PublicProfile.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Exchange(int id)
        {
            ViewBag.BookId = id;
            return View();
        }

        [HttpPost] 
        public ActionResult PerformExchange(Exchange exchange)
        {
            //exchange.Date = DateTime.Now;
            
            //_context.Exchanges.Add(exchange);
           
            //_context.SaveChanges();

            //ViewBag.Message = "Спасибі, " + exchange.Person + ", запит на обмін успішно надіслано!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SetLang(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        [HttpPost]
        public IActionResult SubscribeUser(Subscriber model)
        {
            if (ModelState.IsValid)
            {
                var existingSubscriber =
                    _context.Subscribers.FirstOrDefault(s => s.SubscriberEmail == model.SubscriberEmail);

                if (existingSubscriber == null)
                {
                    model.SubscribitionTime = DateTime.Now;
                    model.Confirmed = false;
                    model.ConfirmationToken = GenerateToken();
                    _context.Subscribers.Add(model);
                    _context.SaveChanges();

                    // Send a confirmation email to the subscriber
                    _emailSender.SendConfirmationEmail(model.SubscriberEmail, model.ConfirmationToken);
                }
            }

            // Return appropriate response, e.g., redirect to a thank you page
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("/confirm-subscription")]
        public IActionResult ConfirmSubscription([FromQuery] string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var subscription = _context.Subscribers.FirstOrDefault(s => s.ConfirmationToken == token);

                if (subscription != null)
                {
                    subscription.Confirmed = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Error");
        }
        private string GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return token;
        }
		[HttpGet]
		public ActionResult AddToWishlist(int bookId)
		{
            var currentUserId = _userManager.GetUserId(User);

			var wishlist =  _context.Wishlists
				.Where(w => w.BookId == bookId && w.UserId == currentUserId)
				.FirstOrDefault();
			var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            
			if (wishlist == null)
			{
				var newWishlist = new Wishlist
				{
					BookId = bookId,
					UserId = currentUserId,
                    
				};

				_context.Wishlists.Add(newWishlist);
				_context.SaveChanges();
				// Отримуємо назву книги
				
				TempData["SuccessMessage"] = ($"Book '{book.Name}' is successfully added to your wishlist!");
				return RedirectToAction("Index", "Home");
			}
			else
			{
				TempData["AlertMessage"] = ($"Book '{book.Name}' isn`t successfully added to your wishlist!");
				return RedirectToAction("Index", "Home");
			}
		}
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Redirect to the return URL
            return Redirect(returnUrl);
        }
    }
}