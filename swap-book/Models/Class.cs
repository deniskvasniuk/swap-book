namespace swap_book.Models
{

    public class Book
    {
        // ID книги
        public int Id { get; set; }
        // назва книги
        public string Name { get; set; }
        // автор книги
        public string Author { get; set; }
        // ціна
        public int Points { get; set; }
    }
}