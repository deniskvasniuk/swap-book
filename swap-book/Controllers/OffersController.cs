using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using Microsoft.AspNetCore.Authorization;
using swap_book.Services;
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
                var books = _context.Books
                    .Include(b => b.Owner)
                    .ToList();

                return View(books);
            }

            var currentUserBooks = _context.Books.Where(b => b.OwnerId == _userManager.GetUserId(User));
            return View(currentUserBooks);
            
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

        public ActionResult EditBook(int? id)
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
			if (!User.IsInRole("Admin") && book?.OwnerId != _userManager.GetUserId(User))
			{
				return Forbid();
			}

			Book existingBook;

			try
			{
				existingBook = await _context.Books.FindAsync(book.BookId);

				if (existingBook == null)
				{
					return NotFound();
				}

				if (book.ImageFile != null)
				{
					var saveImageResult = _fileService.SaveImage(book.ImageFile);
					if (saveImageResult.Item1 == 1)
					{
						var oldImage = book.ImageUrl;
						book.ImageUrl = saveImageResult.Item2;
						_fileService.DeleteImage(oldImage);
					}
				}
				else
				{
					book.ImageUrl = existingBook.ImageUrl;
				}

				book.OwnerId = existingBook.OwnerId;
				book.CreatedAt = existingBook.CreatedAt;
				book.BookLink = existingBook.BookLink;
                book.Exchangeable = existingBook.Exchangeable;

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
        public async Task<ActionResult> AddBook(Book book)
        {
            book.OwnerId = _userManager.GetUserId(User);
            book.BookLink = Guid.NewGuid();

            var selectedCategoryIds = Request.Form["BookCategories"].Select(int.Parse);

            var bookCategories = selectedCategoryIds.Select(categoryId => new BookCategory { BookId = book.BookId, CategoryId = categoryId });

            book.BookCategories = bookCategories.ToList();
            book.CreatedAt = DateTime.Now;
            book.Exchangeable = true;
            book.Owner = await _userManager.GetUserAsync(User);

            var saveImageResult = _fileService.SaveImage(book.ImageFile);
            if (saveImageResult.Item1 == 1)
            {
                var oldImage = book.ImageUrl;
                book.ImageUrl = saveImageResult.Item2;
                var deleteResult = _fileService.DeleteImage(oldImage);
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Index", "Offers");
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

        [Route("book/{bookUrl}")]
        public async Task<ActionResult> BookPage(string bookUrl)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.BookLink == new Guid(bookUrl));

            if (book == null)
            {
                return NotFound();
            }

            book.LoadCategories(_context);

            var result = new BookDetailsViewModel
            {
                Book = book,
                Owner = await _context.Users.FindAsync(book.OwnerId)
            };
            await result.LoadRelatedBooks(book, _context);

            return View(result);
        }
    }
}
