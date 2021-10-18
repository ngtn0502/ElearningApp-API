using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;

namespace API.Controllers
{

    public class ProductsController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product newProduct)
        {
            var product = await _unitOfWork.ProductRepository.PostProductAsync(newProduct);
            await this._unitOfWork.Complete();
            return Ok(product);
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(int query, [FromBody] Product newProduct)
        {
            var product = await _unitOfWork.ProductRepository.EditProductAsync(query, newProduct);

            if (product == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await this._unitOfWork.Complete();
            return Ok(product);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResponse = await _unitOfWork.ProductRepository.DeleteProductAsync(id);

            if (deleteResponse == null) return StatusCode(StatusCodes.Status400BadRequest);

            await this._unitOfWork.Complete();

            return Ok(deleteResponse);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string query, int? pageNumber, int? pageSize)
        {
            var products = await _unitOfWork.ProductRepository.SearchProductAsync(query, pageNumber, pageSize);
            return Ok(products);
        }

        // Useless now - for reference purpose
        // [HttpGet("[action]")]
        // public async Task<IActionResult> Page(int? pageNumber, int? pageSize)
        // {
        //     int currentPageNumber = pageNumber ?? 1;
        //     int currentpageSize = pageSize ?? 10;

        //     var products = await _dbContext.Products.ToListAsync();

        //     var productsPage = products.Skip((currentPageNumber - 1) * currentpageSize).Take(currentpageSize);

        //     var response = new PageResponse
        //     {
        //         PageNumber = pageNumber,
        //         TotalRecords = products.Count,
        //         Products = productsPage
        //     };

        //     return Ok(response);
        // }

        // Pagination
        [HttpGet("[action]")]
        public async Task<IActionResult> Courses(int category, int? pageNumber, int? pageSize)
        {
            return Ok(await _unitOfWork.ProductRepository.GetCoursesAsync(category, pageNumber, pageSize));
        }
    }
}