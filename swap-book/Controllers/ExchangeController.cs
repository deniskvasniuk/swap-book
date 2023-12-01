using Microsoft.AspNetCore.Mvc;
using swap_book.Models;

namespace swap_book.Controllers
{
	public class ExchangeController : Controller
	{
        private readonly DatabaseContext _context;

        public ExchangeController(DatabaseContext context)
        {
			_context=context;
        }
        public IActionResult Index()
		{
			return View(_context.Books);
		}
	}
}
