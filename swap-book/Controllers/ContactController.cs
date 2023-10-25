using Microsoft.AspNetCore.Mvc;
using swap_book.Services;
using swap_book.Models;

namespace swap_book.Controllers
{
	public class ContactController : Controller
	{
        public IActionResult Index()
		{
			return View();
		}
	}
}
