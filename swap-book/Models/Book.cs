using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace swap_book.Models
{

	public class Book
	{
        [ScaffoldColumn(false)]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Owner")]
        public string? OwnerId { get; set; }

        public ApplicationUser? Owner { get; set; }

        public List<Wishlist> Wishlists { get; set; }

        public List<BookCategory> BookCategories { get; set; }
        public Guid? BookLink { get; set; }
        public string? Description { get; set; }
        public void LoadCategories(DatabaseContext context)
        {
	        this.BookCategories = context.BookCategories
		        .Where(bc => bc.BookId == BookId)
		        .Include(bc => bc.Category)
		        .ToList();
        }
	}
}