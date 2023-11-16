using System.ComponentModel.DataAnnotations;

namespace swap_book.Models
{
    public class Mail
    {
        [Required]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Повідомлення")]
        public string Msg { get; set; }
    }
}
