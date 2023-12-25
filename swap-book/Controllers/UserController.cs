using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using swap_book.Services;

namespace swap_book.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
		private readonly DatabaseContext _context;
		private readonly IMessageService _messageService;
		public UserController(UserManager<ApplicationUser> userManager, DatabaseContext context, IMessageService messageService)
		{
			_userManager = userManager;
			_context = context;
			this._messageService = messageService;
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

		public async Task<IActionResult> GetMessages()
		{
			var currentUser = await _userManager.GetUserAsync(HttpContext.User);

			var messages = await _messageService.GetMessages(currentUser);

			return View(messages);
		}
        [HttpPost]
        public async Task<IActionResult> SendMessage(string message, string recipientId )
        {
            try
            {
       
                // Отримайте значення messageText
                //var messageText = message["messageText"].ToString();
                // Отримайте користувачів з контексту або через параметри
                var sender = await _userManager.GetUserAsync(HttpContext.User); ; // отримати SenderUser;


                //await _messageService.SendMessage(sender, recipient, messageText);

                // Поверніть успішну відповідь
                return Redirect(HttpContext.Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
