using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swap_book.Models
{

    public class Book
    {

        public int BookId { get; set; }

       
        public string Name { get; set; }
        
        public string Author { get; set; }
        
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}