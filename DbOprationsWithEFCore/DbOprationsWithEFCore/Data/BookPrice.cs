namespace DbOprationsWithEFCore.Data
{
    public class BookPrice
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
