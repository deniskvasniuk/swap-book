using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using System.Linq;
using System.Net;

namespace swap_book.Controllers
{
	public class WishlistController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly DatabaseContext _context;

		public WishlistController(UserManager<ApplicationUser> userManager, DatabaseContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> GetWishlist(string userId)
		{
            // Check for the refresh parameter and clear it
            if (Request.Query.ContainsKey("refresh"))
            {
                // Redirect to the same action without the refresh parameter to clear it from the URL
                return RedirectToAction("GetWishlist", new { userId });
            }
            var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}
            var currentUserWishList = _context.Wishlists
                .FirstOrDefault(w => w.UserId == userId);
            if (currentUserWishList == null)
            {
                TempData["AlertMessage"] = ($"Your wishlist is empty! Add new books first!");
                return Redirect(HttpContext.Request.Headers["Referer"].ToString());
            }
            var wishlists = _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Book);

            var books = wishlists
                .Select(w => w.Book)
                .ToList();

            return View("~/Views/User/GetWishlist.cshtml", books);
		}

		public async Task<IActionResult> DeleteWish(int bookId)
		{
			var user = await _userManager.GetUserAsync(User);

			var wishlistItem = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == user.Id && w.BookId == bookId);

			_context.Wishlists.Remove(wishlistItem);
			await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = ($"Your wish is successfully deleted");
            return RedirectToAction("GetWishlist", new { userId = user.Id, refresh = true, successMessage = TempData["SuccessMessage"] });
        }
	}
}
