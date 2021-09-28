using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Entities;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{

    public class ProductsController : BaseAPIController
    {
        private readonly DataContext _dbContext;
        public ProductsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =  await _dbContext.Products.Include(a => a.ProductDetails).ToListAsync();
            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products =  await _dbContext.Products.FindAsync(id);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Products product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(int query, [FromBody] Products newProduct)
        {
            var product = await _dbContext.Products.FindAsync(query);

            if (product == null) return StatusCode(StatusCodes.Status400BadRequest);

            product.Name = newProduct.Name;
            product.Description = newProduct.Description;
            product.Rating = newProduct.Rating;
            product.Price = newProduct.Price;
            product.CategoryId = newProduct.CategoryId;
            product.ImageUrl = newProduct.ImageUrl;
            product.Instructor = newProduct.Instructor;

            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(i=> i.Id == id);

            if (product == null) return StatusCode(StatusCodes.Status400BadRequest);

            _dbContext.Remove(product);

            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}