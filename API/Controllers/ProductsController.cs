using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;

namespace API.Controllers
{

    public class ProductsController : BaseAPIController
    {
        private readonly DataContext _dbContext;
        private readonly IProductRepository _productRepository;

        public ProductsController(DataContext dbContext, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = await _productRepository.GetProductByIdAsync(id);
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Products product)
        {
            return Ok(await _productRepository.PostProductAsync(product));
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(int query, [FromBody] Products newProduct)
        {
            var product = await _productRepository.EditProductAsync(query, newProduct);

            if (product == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Ok(product);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResponse = await _productRepository.DeleteProductAsync(id);

            if (deleteResponse == null) return StatusCode(StatusCodes.Status400BadRequest);

            return Ok(deleteResponse);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string query)
        {
            var products = await _productRepository.SearchProductAsync(query);
            return Ok(products);
        }

        // Useless now - for reference purpose
        [HttpGet("[action]")]
        public async Task<IActionResult> Page(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 10;

            var products = await _dbContext.Products.ToListAsync();

            var productsPage = products.Skip((currentPageNumber - 1) * currentpageSize).Take(currentpageSize);

            var response = new PageResponse
            {
                PageNumber = pageNumber,
                TotalRecords = products.Count,
                Products = productsPage
            };

            return Ok(response);
        }

        // Pagination
        [HttpGet("[action]")]
        public async Task<IActionResult> Courses(int category, int? pageNumber, int? pageSize)
        {
            return Ok(await _productRepository.GetCoursesAsync(category, pageNumber, pageSize));
        }
    }
}