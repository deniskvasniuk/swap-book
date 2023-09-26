using Microsoft.AspNetCore.Mvc;
using swap_book.Models;
using System.Diagnostics;

namespace swap_book.Controllers
{
    public class HomeController : Controller
    {
        BookContext db = new BookContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
 
            IEnumerable<Book> books = db.Books;
            ViewBag.Books = books;
            return View();
        }

        public IActionResult Privacy()
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

        [HttpPost] // Додано атрибут, оскільки це POST-запит
        public ActionResult PerformExchange(Exchange exchange)
        {
            exchange.Date = DateTime.Now;
            // Додаємо інформацію про покупку в базу даних
            db.Exchanges.Add(exchange);
            // Зберігаємо в БД всі зміни
            db.SaveChanges();
            ViewBag.Message = "Спасибі, " + exchange.Person + ", запит на обмін успішно надіслано!";
            return View("ExchangeConfirmation");
        }

    }
}