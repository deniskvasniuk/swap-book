using System.Data.Entity;
namespace swap_book.Models
{
    public class BookDbInitializer : DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext db)
        {

            db.Books.Add(new Book { Name = "Кобзар", Author = "Тарас Шевченко", Points = 15 });
            db.Books.Add(new Book { Name = "Маруся", Author = "Іван Франко", Points = 20 });
            db.Books.Add(new Book { Name = "Кайдашева сім'я", Author = "Іван Нечуй-Левицький", Points = 30 });
            base.Seed(db);
        }
        }
}
