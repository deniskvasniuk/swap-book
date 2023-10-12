using Microsoft.AspNetCore.Mvc;

namespace swap_book.Controllers
{
	public class ServicesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
