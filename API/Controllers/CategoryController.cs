using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{

    public class CategoryController : BaseAPIController
    {
        private readonly DataContext _dbContext;
        public CategoryController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories =  await _dbContext.Category.Include(a=>a.Products).ToListAsync();
            return Ok(categories);
        }

        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category =  await _dbContext.Category.FindAsync(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}