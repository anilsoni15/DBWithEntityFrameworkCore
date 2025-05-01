using DbOprationsWithEFCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbOprationsWithEFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        //Get Value from sql query on database 
        [HttpPost("GetBooksBySqlQuery/{id}")]
        public async Task<ActionResult> GetBooksUsingDatabase([FromRoute] int id, [FromBody] int? noOfPages)
        {
            var books = await appDbContext.Database.ExecuteSqlAsync($"update books set NoOfPages={noOfPages} where Id={id}");
                return Ok(books);
        }
        //Get Value form SQL Proc from  
        [HttpGet("GetBooksByProc")]
        public async Task<ActionResult> GetBooksByProc()
        {
            var columnName = "Id";
            var columnValue = "10";
            var perameter = new SqlParameter("columnValue", columnValue);
            //sql proc
            var books = await appDbContext.Books.FromSql($"exec SP_GETAllBooks").ToListAsync();
            var books1 = await appDbContext.Books.FromSql($"exec SP_GetBookById {perameter}").ToListAsync();
            return Ok(books);
        }

        //Get Value form SQL from  
        [HttpGet("GetAllSqlBooks")]
        public async Task<ActionResult> GetAllSqlBooks()
        {
            var columnName = "Id";
            var columnValue = "10";
            var perameter = new SqlParameter("columnValue", columnValue);
            //only with sql query
            var books = await appDbContext.Books.FromSql($"select * from Books").Where(x=>x.Id<15).ToListAsync();

            // sql query with perameter
            var books1 = await appDbContext.Books.FromSqlRaw($"select * from Books where {columnName}=@columnValue",perameter).ToListAsync();
            return Ok(books);
        }

        //Lazy loading 
        [HttpGet("GetAllLazyBooks")]
        public async Task<ActionResult> GetAllLazyBooks()
        {
            var books = await appDbContext.Books.FirstAsync();
            var author = books.Author;
            return Ok(books);
        }
        //Egar loading with include | include than 
        [HttpGet("GetBookswithInclude")]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = await appDbContext
            .Books
            .Include(x => x.Author)
            // .Include(y => y.Language)
            .ToListAsync();
            return Ok(books);
        }
        //Explicit loading with Refernece | colllection | load
        [HttpGet("GetExpBooks")]
        public async Task<ActionResult> GetExpBooks()
        {
            var book = await appDbContext.Books.FirstAsync();
            //await appDbContext.Entry(book).Reference(x => x.Language).LoadAsync();
            await appDbContext.Entry(book).Reference(x => x.Author).LoadAsync();

            return Ok(book);
        }
        [HttpGet("GetAllLanguages")]
        public async Task<ActionResult> GetAllLanguages()
        {
            var languages = await appDbContext.Languages.ToListAsync();
            foreach (var lang in languages)
            {
                await appDbContext.Entry(lang).Collection(x => x.Books).LoadAsync();
            }
            return Ok(languages);
        }
        [HttpGet("GetLanguages")]
        public async Task<ActionResult> GetLanguagesWithBooks()
        {
            var languages = await appDbContext
            .Languages
            // .Include(y => y.Books)
            .ToListAsync();
            return Ok(languages);
        }
        [HttpGet("BooksAsync")]
        public async Task<IActionResult> GetBooksAsync()
        {
            var books = await appDbContext.Books.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                NoOfPages = x.NoOfPages,
                Author = x.Author != null ? x.Author.Name : "NA",
                // Labgaue = x.Language != null ? x.Language.Title : "English"

            }).ToListAsync();

            return Ok(books);
        }
        [HttpGet("GetBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await (from books in appDbContext.Books
                                select books).AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book model)
        {
            var auth = new BookAuthor
            {
                Name = "Raj",
                Email = "rja@gmail.com"
            };
            model.Author = auth;
            appDbContext.Books.Add(model);
            await appDbContext.SaveChangesAsync();
            return Ok(model);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> model)
        {
            appDbContext.Books.AddRange(model);
            await appDbContext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] Book model)
        {
            var book = appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = model.Title;
            book.Description = model.Description;

            await appDbContext.SaveChangesAsync();
            return Ok(model);
        }
        [HttpPut("bulkUpdate")]
        public async Task<IActionResult> UpdateBookInBulk()
        {
            await appDbContext.Books
                .Where(x => x.NoOfPages > 50)
                .ExecuteUpdateAsync(x => x
            .SetProperty(p => p.Description, "This is Description2")
            .SetProperty(p => p.Title, p => p.Title + " Test2"));

            return Ok();
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookByIdAsync([FromRoute] int bookId)
        {
            var book = new Book { Id = bookId };
            appDbContext.Entry(book).State = EntityState.Deleted;
            await appDbContext.SaveChangesAsync();
            return Ok();

            //var book = appDbContext.Books.FirstOrDefault(x => x.Id == bookId);
            //if(book==null)
            //{
            //    return NotFound();
            //}
            //appDbContext.Books.Remove(book);
            //await appDbContext.SaveChangesAsync();
            //return Ok();
        }
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteBooksInBulkAsync()
        {
            //var books = await appDbContext.Books.Where(x => x.Id < 5).ToListAsync();
            //appDbContext.Books.RemoveRange(books);
            //await appDbContext.SaveChangesAsync();
            var rocords = await appDbContext.Books.Where(x => x.Id < 10).ExecuteDeleteAsync();
            return Ok(rocords);
        }
    }
}
