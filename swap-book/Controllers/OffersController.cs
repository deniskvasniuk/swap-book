using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using swap_book.Services;
using EntityState = System.Data.Entity.EntityState;
using Microsoft.AspNetCore.Identity;


namespace swap_book.Controllers
{
    [Authorize]
    public class OffersController : Controller
	{
        private readonly DatabaseContext _context;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        public OffersController(DatabaseContext context, IFileService fileService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _fileService = fileService;
            _userManager = userManager;
        }

		public IActionResult Index()
		{
            if (User.IsInRole("Admin"))
            {
                return View(_context.Books);
            }
            else
            {
                var currentUserBooks = _context.Books.Where(b => b.OwnerId == User.Identity.Name);
                return View(currentUserBooks);
            }
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
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public ActionResult EditBook(int? id)
        {

           var book = _context.Books.Find(id);
            // Check if the user is an administrator or the owner of the book
            if (!User.IsInRole("Admin") && book?.OwnerId != User.Identity.Name)
            {
                return Forbid(); // Return 403 Forbidden if not authorized
            }


            if (id == null)
            {
                return NotFound();
            }

            if (book == null)
            {
                return NotFound();
            }

            return View(book);

        }
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(Book book)
        {
            if (!User.IsInRole("Admin") && book?.OwnerId != User.Identity.Name)
            {
                return Forbid(); 
            }

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

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public ActionResult AddBook()
        {
            var model = new Book();
            model.BookCategories = new List<BookCategory>(); 
            ViewBag.Categories = _context.Categories.ToList(); 
            return View(model);
        }


        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            book.OwnerId = _userManager.GetUserId(User);

            try
            {
                var selectedCategoryIds = Request.Form["BookCategories"].Select(int.Parse);

                var bookCategories = selectedCategoryIds.Select(categoryId => new BookCategory { BookId = book.BookId, CategoryId = categoryId });

                book.BookCategories = bookCategories.ToList();

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



        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public ActionResult DeleteBook(int? id)
        {
            var book = _context.Books.Find(id);

            if (!User.IsInRole("Admin") && book?.OwnerId != _userManager.GetUserId(User))
            {
                return Forbid(); 
            }
            if (id == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost, ActionName("DeleteBook")]
        public ActionResult DeleteConfirmed(int? id)
        {
            var book = _context.Books.Find(id);

            if (!User.IsInRole("Admin") && book?.OwnerId != _userManager.GetUserId(User))
            {
                return Forbid(); 
            }
            if (id == null)
            {
                return NotFound();
            }
   
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
