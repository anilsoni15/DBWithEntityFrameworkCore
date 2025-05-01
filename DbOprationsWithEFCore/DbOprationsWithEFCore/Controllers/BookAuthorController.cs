using DbOprationsWithEFCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOprationsWithEFCore.Controllers
{
    [Route("api/Controller")]
    public class BookAuthorController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet("GetAllBookAuthors")]
        public async Task<IActionResult> GetBookAuthors()
        {
            var bookAuthors =await appDbContext.Authors.AsNoTracking().ToListAsync();
            return Ok(bookAuthors);
        }
    }
}
