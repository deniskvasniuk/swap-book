using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swap_book.Models
{

	public class Book
	{
		[ScaffoldColumn(false)]
		public int BookId { get; set; }

		[Required]
		[Display(Name = "Назва")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Автор")]
		public string Author { get; set; }

		public string? ImageUrl { get; set; }

		[NotMapped]
		public IFormFile ImageFile { get; set; }

		[Display(Name = "Власник")]
		public string OwnerId { get; set; }

		public ApplicationUser Owner { get; set; }
	}
}