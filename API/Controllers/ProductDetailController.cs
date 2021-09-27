using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{

    public class ProductDetailController : BaseAPIController
    {
        private readonly DataContext _dbContext;
        public ProductDetailController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }


        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDeTail(int id)
        {
            var productDetail =  await _dbContext.ProductDetail.FindAsync(id-1);
            return Ok(productDetail);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductDetail productDetail)
        {
            await _dbContext.ProductDetail.AddAsync(productDetail);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        
        [HttpPut("[action]")]
        public async Task<IActionResult> Edit(int query, [FromBody] ProductDetail newProductDetail)
        {
            var productDetail = await _dbContext.ProductDetail.FindAsync(query);

            if (productDetail == null) return StatusCode(StatusCodes.Status400BadRequest);

            productDetail.Details = newProductDetail.Details;


            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }


    }
}