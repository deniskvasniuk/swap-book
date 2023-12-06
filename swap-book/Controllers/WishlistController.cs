using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;

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
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			var booksInWishlist = await _context.Books
				.Where(b => b.Wishlists.Any(w => w.UserId == user.Id))
				.ToListAsync();

			return View("~/Views/User/GetWishlist.cshtml",booksInWishlist);
		}
	}
}
