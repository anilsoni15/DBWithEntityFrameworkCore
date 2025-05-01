using DbOprationsWithEFCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOprationsWithEFCore.Controllers
{
    [Route("api/Controller")]
    public class LanguageController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet("GetLanguages")]
        public async Task<IActionResult> GetLanguages()
        {
            //var result = await appDbContext.Languages.ToListAsync();
            var result = await (from languages in appDbContext.Languages select languages)
                .AsNoTracking()
                .ToListAsync();
            return Ok(result);
        }
        [HttpPost("language")]
        public async Task<IActionResult> AddLanguage([FromBody] Language language)
        {
            if (language == null)
            {
                return NotFound();
            }
            var lang = new Language
            {
                Title = language.Title,
                Description = language.Description,
            };
            appDbContext.Languages.Add(lang);
            await appDbContext.SaveChangesAsync();
            return Ok(language);
        }
        [HttpPut("UpdateLanguage/{id}")]
        public async Task<IActionResult> UpdateLanguage([FromRoute] int id, [FromBody] Language language)
        {
            var lang =await appDbContext.Languages.FirstOrDefaultAsync(x => x.Id == id);
            if (lang != null)
            {
                lang.Title = language.Title;
                lang.Description = language.Description;
                await appDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpDelete("{langaugeId}")]
        public async Task<IActionResult> DeleteLangaugeById([FromRoute] int langaugeId)
        {
            var langobj = await appDbContext.Languages.FirstOrDefaultAsync(x => x.Id == langaugeId);
            if (langobj == null)
            {
                return NotFound();
            }
            appDbContext.Languages.Remove(langobj);
            await appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
