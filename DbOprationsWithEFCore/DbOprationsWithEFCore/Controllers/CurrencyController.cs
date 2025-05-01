using DbOprationsWithEFCore.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOprationsWithEFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public CurrencyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencies()
        {
           // var result = _appDbContext.Currencies.ToList();
           var result =await (from currencies  in _appDbContext.Currencies
                         select new
                         {
                             CurrencyId=currencies.Id,
                             CurrencyName=currencies.Title
                         }).ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetAllCurrenciesAsync")]
        public async Task<IActionResult> GetAllCurrenciesAsync()
        {
            var result = await _appDbContext.Currencies.ToListAsync();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetCurrencyById([FromRoute] int id)
        {
            var result = _appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByName([FromRoute] string name, [FromQuery] string? description)
        {
            //var result = await _appDbContext.Currencies
            //    .FirstOrDefaultAsync(x => x.Title == name 
            //    && (string.IsNullOrEmpty(description) 
            //    || x.Description == description));
            var result = await _appDbContext.Currencies
                .Where(x=>x.Title == name && 
                (string.IsNullOrEmpty(description)||x.Description==description)).ToListAsync();
            return Ok(result);
        }
        [HttpPost("all")]
        public async Task<IActionResult> GetCurrenciesByIds([FromBody] List<int> ids)
        {
        //    var ids = new List<int>() { 1, 4, 3 };
            //var result =await _appDbContext.Currencies
              //  .Where(x=>ids.Contains(x.Id)).ToListAsync();
              var result = await _appDbContext.Currencies
                .Where(x=>ids.Contains(x.Id))
                .Select(x=>new Currency()
                {
                    Id=x.Id,
                    Title=x.Title,
                })
                .ToListAsync();
            return Ok(result);
        }
    }
}
