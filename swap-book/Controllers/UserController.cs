using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;

namespace swap_book.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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



    }
}
