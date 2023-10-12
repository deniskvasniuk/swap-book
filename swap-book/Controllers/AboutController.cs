using Microsoft.AspNetCore.Mvc;

namespace swap_book.Controllers
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
