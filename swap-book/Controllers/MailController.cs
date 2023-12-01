using Microsoft.AspNetCore.Mvc;
using swap_book.Models;
using swap_book.Services;

namespace swap_book.Controllers
{
    public class MailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<MailController> _logger;

        public MailController(IEmailSender emailSender, ILogger<MailController> logger)
        {
            _emailSender = emailSender;
            _logger = logger;

        }

        [HttpPost]
        public IActionResult Feedback(Mail model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            try
            {
                _emailSender.SendMail(model.FirstName,model.LastName, model.Email, model.Msg);
                _logger.LogInformation($"Mail: email is successfully sent to {model.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
