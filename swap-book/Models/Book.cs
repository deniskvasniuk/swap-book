using System.ComponentModel.DataAnnotations;

namespace swap_book.Models
{

    public class Book
    {

        public int BookId { get; set; }
       
        public string Name { get; set; }
        
        public string Author { get; set; }
        
        public string ImageUrl { get; set; }


    }
}