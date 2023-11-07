using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using System.Data.Entity;
using swap_book.Services;
using EntityState = System.Data.Entity.EntityState;


namespace swap_book.Controllers
{

	public class OffersController : Controller
	{
        private readonly BookContext _context;
        private readonly IFileService _fileService;
        public OffersController(BookContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

		public IActionResult Index()
		{
			return View(_context.Books);
		}

        public IActionResult Exchange()
        {
            return View();
        }
        public ActionResult BookView(int id)
        {
            var book = _context.Books.Find(id);
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

            Book book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(Book book)
        {
            try
            {
                if (book.ImageFile != null)
                {
                    var saveImageResult = _fileService.SaveImage(book.ImageFile);
                    if (saveImageResult.Item1 == 1)
                    {
                        var oldImage = book.ImageUrl;
                        book.ImageUrl = saveImageResult.Item2;
                        var deleteResult = _fileService.DeleteImage(oldImage);
                    }
                }
                else
                {
                    var existingBook = await _context.Books.FindAsync(book.BookId);
                    _context.Entry(existingBook).State = Microsoft.EntityFrameworkCore.EntityState.Detached; 
                    book.ImageUrl = existingBook.ImageUrl;
                }

                _context.Attach(book); 
                _context.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified; 

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook([Bind("Name", "Author", "ImageUrl", "ImageFile")] Book book)
        {
            try
            {
                var saveImageResult = _fileService.SaveImage(book.ImageFile);
                if (saveImageResult.Item1 == 1)
                {
                    var oldImage = book.ImageUrl;
                    book.ImageUrl = saveImageResult.Item2;
                    var deleteResult = _fileService.DeleteImage(oldImage);
                }
                _context.Books.Add(book);
                _context.SaveChanges();
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
            Book book = _context.Books.Find(id);
            return View(book);
        }
        [HttpPost, ActionName("DeleteBook")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Book book = _context.Books.Find(id);
   
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
