using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using API.Interfaces;

namespace API.Controllers
{

    public class CategoryController : BaseAPIController
    {
        private readonly DataContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(DataContext dbContext, ICategoryRepository categoryRepository)
        {
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoryAsync();
            return Ok(categories);
        }

        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null) return StatusCode(StatusCodes.Status400BadRequest);

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] Category newCategory)
        {
            var category = await _categoryRepository.PostCategoryAsync(newCategory);

            return Ok(category);
        }
    }
}