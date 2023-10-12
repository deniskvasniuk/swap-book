using Microsoft.AspNetCore.Mvc;
using swap_book.Models;
using System.Diagnostics;

namespace swap_book.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookContext db;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BookContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
	        
            return View();
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
            
            db.Exchanges.Add(exchange);
           
            db.SaveChanges();

            ViewBag.Message = "Спасибі, " + exchange.Person + ", запит на обмін успішно надіслано!";
            return View("ExchangeConfirmation");
        }

    }
}