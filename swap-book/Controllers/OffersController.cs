using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using EntityState = System.Data.Entity.EntityState;


namespace swap_book.Controllers
{

	public class OffersController : Controller
	{
        private readonly BookContext db;
        public OffersController(BookContext context)
        {
            db = context;
        }

		public IActionResult Index()
		{
			return View(db.Books);
		}

        public IActionResult Exchange()
        {
            return View();
        }
        public ActionResult BookView(int id)
        {
            var book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            db.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            try
            {
                db.Books.Add(book);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Book book = db.Books.Find(id);
            return View(book);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Book book = db.Books.Find(id);
   
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
