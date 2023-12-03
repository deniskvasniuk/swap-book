using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swap_book.Models
{
    public class Category
    {
        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }

        public List<BookCategory> BookCategories { get; set; }


    }
    public static class DefaultCategories
    {
        public static List<Category> GetDefaultCategories()
        {
            return new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Adventure" },
                new Category { CategoryId = 2,CategoryName = "Biography" },
                new Category { CategoryId = 3,CategoryName = "Crime" },
                new Category { CategoryId = 4,CategoryName = "Drama" },
                new Category { CategoryId = 5,CategoryName = "Fantasy" },
                new Category { CategoryId = 6,CategoryName = "Historical Fiction" },
                new Category { CategoryId = 7,CategoryName = "Horror" },
                new Category { CategoryId = 8,CategoryName = "Humor" },
                new Category { CategoryId = 9, CategoryName = "Mystery" },
                new Category { CategoryId = 10,CategoryName = "Poetry" },
                new Category { CategoryId = 11,CategoryName = "Romance" },
                new Category { CategoryId = 12,CategoryName = "Science Fiction" },
                new Category { CategoryId = 13,CategoryName = "Thriller" },
                new Category { CategoryId = 14,CategoryName = "Cookbook" },
                new Category { CategoryId = 15,CategoryName = "Religious" },
                new Category { CategoryId = 16,CategoryName = "Travel" },
                new Category { CategoryId = 17,CategoryName = "Adventure" },
                new Category { CategoryId = 18,CategoryName = "Self-Help" },
                new Category { CategoryId = 19,CategoryName = "Business" },
                new Category { CategoryId = 20,CategoryName = "Health" },
                new Category { CategoryId = 21,CategoryName = "Fitness" },
                new Category { CategoryId = 22,CategoryName = "Education" },
                new Category { CategoryId = 23,CategoryName = "History" },
                new Category { CategoryId = 24,CategoryName = "Biography" },
                new Category { CategoryId = 25,CategoryName = "Art" },
                new Category { CategoryId = 26,CategoryName = "Music" },
                new Category { CategoryId = 27,CategoryName = "Philosophy" },
                new Category { CategoryId = 28,CategoryName = "Psychology" },
                new Category { CategoryId = 29,CategoryName = "Technology" }
            };
        }
    }
}
