using LotteriaAPI.Data;
using LotteriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly LotteDbContext _lotteDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(LotteDbContext lotteDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _lotteDbContext = lotteDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var category = await _lotteDbContext.Category.ToListAsync();
            return Ok(category);
        }

        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] CategoryViewModel _category)
        {
            var category = new Category
            {
                Name = _category.Name,
            };

            category.CategoryImage = await ConvertImageBase.ImageToBase64(_category.CategoryImageView);

            await _lotteDbContext.Category.AddAsync(category);
            await _lotteDbContext.SaveChangesAsync();
            return Ok(category);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            var category = await _lotteDbContext.Category.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromForm] CategoryViewModel _category)
        {
            var category = await _lotteDbContext.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = _category.Name;
            if(_category.CategoryImageView != null) {
                category.CategoryImage = await ConvertImageBase.ImageToBase64(_category.CategoryImageView);
            }
           
            await _lotteDbContext.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var category = await _lotteDbContext.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _lotteDbContext.Category.Remove(category);
            await _lotteDbContext.SaveChangesAsync();
            return Ok(category);
        }



    }
}
