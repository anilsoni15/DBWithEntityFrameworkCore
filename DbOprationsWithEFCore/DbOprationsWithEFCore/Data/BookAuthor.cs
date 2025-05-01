using System.ComponentModel.DataAnnotations;

namespace DbOprationsWithEFCore.Data
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
