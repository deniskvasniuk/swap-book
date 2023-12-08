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

            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);

            var existingWishlistEntry = _context.Wishlists
                .FirstOrDefault(w => w.UserId == currentUserId && w.BookId == bookId);

            if (existingWishlistEntry == null)
            {
                // Створити новий вішліст, якщо він не існує
                var newWishlist = new Wishlist
                {
                    UserId = currentUserId,
                    BookId = bookId
                };

                _context.Wishlists.Add(newWishlist);
                _context.SaveChanges();


                book.Wishlists.Add(newWishlist);
                _context.SaveChangesAsync();

                UpdateBookWishlist(newWishlist);

                TempData["SuccessMessage"] = ($"Book is successfully added to your wishlist!");
            }
            else
            {
                // Книга вже є у вішлісті, відобразити відповідне повідомлення
                TempData["AlertMessage"] = ($"Book is already in your wishlist!");
            }

			return Redirect(HttpContext.Request.Headers["Referer"].ToString());
		}
        public IActionResult SetCultureCookie(string cltr)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Redirect to the return URL
            return LocalRedirect("/");
        }
        public void UpdateBookWishlist(Wishlist wishlist)
        {
            //_context.BookWishlist.Add(new BookWishlist
            //{
            //    BookId = wishlist.BookId,
            //    WishlistId = wishlist.Id
            //});

            _context.SaveChanges();
        }
    }
}