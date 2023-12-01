using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using swap_book.Models;

namespace swap_book.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public UserController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> PublicProfile(Guid publicProfileLink)
        {
            ApplicationUser user = await _userManager.FindByPublicProfileLinkAsync(publicProfileLink.ToString());

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    }
}
