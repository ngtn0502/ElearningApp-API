using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;

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
            var products = await _dbContext.Products.Include(a => a.ProductDetails).ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var products = await _dbContext.Products.FindAsync(id);
            return Ok(products);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Products product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return Ok(product);
        }

        [Authorize]
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
            return Ok(product);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(i => i.Id == id);

            if (product == null) return StatusCode(StatusCodes.Status400BadRequest);

            _dbContext.Remove(product);

            await _dbContext.SaveChangesAsync();

            var returnedObj = new DeleteResponse
            {
                id = product.Id
            };

            return Ok(returnedObj);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string query)
        {
            var products = await (from product in _dbContext.Products
                                  where
                                  (
                                  product.Name.ToLower().Contains
                                  (query.ToLower())
                                  ||
                                  product.Description.ToLower().Contains(query.ToLower())
                                  ||
                                  product.Instructor.ToLower().Contains(query.ToLower())
                                  )
                                  select new
                                  {
                                      Id = product.Id,
                                      Name = product.Name,
                                      Description = product.Description,
                                      Rating = product.Rating,
                                      Price = product.Price,
                                      CategoryId = product.CategoryId,
                                      ImageUrl = product.ImageUrl,
                                      Instructor = product.Instructor,
                                  }).ToListAsync();
            return Ok(products);
        }

        // Pagination
        [HttpGet("[action]")]
        public async Task<IActionResult> Page(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 10;

            var products = await _dbContext.Products.Include(a => a.ProductDetails).ToListAsync();

            var productsPage = products.Skip((currentPageNumber - 1) * currentpageSize).Take(currentpageSize);

            var response = new PageResponse
            {
                PageNumber = pageNumber,
                TotalRecords = products.Count,
                Products = productsPage
            };

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Courses(int category, int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 3;

            var products = await _dbContext.Products.ToListAsync();

            if (category != 0)
            {
                products = await _dbContext.Products.Where(x => x.CategoryId == category).ToListAsync();
            }

            var productsPage = products
            .Skip((currentPageNumber - 1) * currentpageSize)
            .Take(currentpageSize);

            var response = new PageResponse
            {
                PageNumber = currentPageNumber,
                TotalRecords = products.Count,
                Products = productsPage
            };

            return Ok(response);
        }
    }
}