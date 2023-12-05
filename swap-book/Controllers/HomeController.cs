using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using swap_book.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using swap_book.Services;
using swap_book.ViewModels;

namespace swap_book.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IEmailSender _emailSender;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {

		   
		   var books = _context.Books
			   .Include(b => b.Owner)
			   .Include(b => b.Wishlists)
			   .ToList();

		   // Select the latest books
		   var latestBooks = books
			   .OrderByDescending(b => b.CreatedAt)
			   .Take(10);

		   // Prepare an instance of the ViewModel
		   var viewModel = new IndexViewModel(books, latestBooks, User.Identity.Name);

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
            exchange.Date = DateTime.Now;
            
            _context.Exchanges.Add(exchange);
           
            _context.SaveChanges();

            ViewBag.Message = "Спасибі, " + exchange.Person + ", запит на обмін успішно надіслано!";
            return View("ExchangeConfirmation");
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
    }
}