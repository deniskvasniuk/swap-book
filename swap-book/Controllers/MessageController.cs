using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NuGet.Protocol.Plugins;
using swap_book.Models;
using swap_book.Services;
using Message = swap_book.Models.Message;

namespace swap_book.Controllers
{
    public class MessageController : Controller
    {
       private readonly UserManager<ApplicationUser> _userManager;
       private readonly DatabaseContext _context;
       private readonly IMessageService _messageService;

        public MessageController(UserManager<ApplicationUser> userManager, DatabaseContext context, IMessageService messageService)
       {
            _context=context;
            _userManager=userManager;
            this._messageService = messageService;

       }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message msg)
        {

            try
            {
                var sender = await _userManager.GetUserAsync(User);
                string content = Request.Form["userInput"];
                string recipientId = Request.Form["recipientId"];
                var recipient = await _userManager.FindByIdAsync(msg.RecipientId);

                if (sender.Id == recipientId)
                {
                    TempData["AlertMessage"] = ($"You cant send message to yourself!");
                    return Redirect(HttpContext.Request.Headers["Referer"].ToString());
                }
                
                await _messageService.SendMessage(sender, recipient, content);
                TempData["SuccessMessage"] = ($"Message is successfully send!");

                return Redirect(HttpContext.Request.Headers["Referer"].ToString());
            }

            catch (Exception ex)
            {
                
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> GetUnreadMessages()
        {
            var userId = _userManager.GetUserId(User); // Retrieve the current user's ID

            var unreadMessages = await _context.Messages
                .Where(m => m.RecipientId == userId && m.IsRead == false)
                .ToListAsync();


            return View(unreadMessages); 
        }
    }
}
