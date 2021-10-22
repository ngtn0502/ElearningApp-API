using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Microsoft.AspNetCore.Http;
using API.Interfaces;

namespace API.Controllers
{

    public class CategoryController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoryAsync();
            return Ok(categories);
        }

        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(id);

            if (category == null) return StatusCode(StatusCodes.Status400BadRequest);

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category newCategory)
        {
            var category = await _unitOfWork.CategoryRepository.CreateCategoryAsync(newCategory);

            await this._unitOfWork.Complete();

            return Ok(category);
        }
    }
}