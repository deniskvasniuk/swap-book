using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using System.Data.Entity;

namespace swap_book.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
		private readonly DatabaseContext _context;
		public UserController(UserManager<ApplicationUser> userManager, DatabaseContext context)
		{
			_userManager = userManager;
			_context = context;
		}


		public async Task<IActionResult> PublicProfile(string publicProfileLink)
        {
            var result = await _userManager.Users.FirstOrDefaultAsync(u => u.PublicProfileLink == new Guid(publicProfileLink));
            ApplicationUser user = result;

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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

			return View(booksInWishlist);
		}

	}
}
