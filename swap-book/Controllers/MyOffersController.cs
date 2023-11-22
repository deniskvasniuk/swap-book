using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using swap_book.Models;

namespace swap_book.Controllers
{

	public class MyOffersController : Controller
	{
		private readonly DatabaseContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		public MyOffersController(DatabaseContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var currentUser = await _userManager.GetUserAsync(User);
			var myOffers = _context.Books.Where(b => b.OwnerId == currentUser.Id).ToList();
			return View(myOffers);
		}
	}
}
