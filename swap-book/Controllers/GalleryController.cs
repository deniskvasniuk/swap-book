using Microsoft.AspNetCore.Mvc;

namespace swap_book.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
