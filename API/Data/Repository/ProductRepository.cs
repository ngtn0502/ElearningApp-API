using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.Helpers;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        async Task<IQueryable<Product>> IProductRepository.GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.AsQueryable();
        }

        async Task<Product> IProductRepository.GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }


        void IProductRepository.Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        async Task<Product> IProductRepository.PostProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return product;
        }

        async Task<Product> IProductRepository.EditProductAsync(int id, Product newProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return null;

            product.Name = newProduct.Name;
            product.Description = newProduct.Description;
            product.Rating = newProduct.Rating;
            product.Price = newProduct.Price;
            product.CategoryId = newProduct.CategoryId;
            product.ImageUrl = newProduct.ImageUrl;
            product.Instructor = newProduct.Instructor;
            product.Language = newProduct.Language;


            return product;
        }


        async Task<DeleteResponse> IProductRepository.DeleteProductAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(i => i.Id == id);

            if (product == null) return null;

            _context.Remove(product);

            var returnedObj = new DeleteResponse
            {
                id = product.Id
            };

            return returnedObj;
        }

        async Task<PageResponse> IProductRepository.SearchProductAsync(string query, int? pageNumber, int? pageSize)
        {

            var products = await (from product in _context.Products
                                  where
                                  (
                                  product.Name.ToLower().Contains
                                  (query.ToLower())
                                  ||
                                  product.Description.ToLower().Contains(query.ToLower())
                                  ||
                                  product.Instructor.ToLower().Contains(query.ToLower())
                                  )
                                  select product).ToListAsync();


            return PagedList.CreatePagedResponse(products, pageNumber, pageSize);
        }

        async Task<PageResponse> IProductRepository.GetCoursesAsync(int category, int? pageNumber, int? pageSize)
        {

            var products = await _context.Products.ToListAsync();

            if (category != 0)
            {
                products = await _context.Products.Where(x => x.CategoryId == category).ToListAsync();
            }

            return PagedList.CreatePagedResponse(products, pageNumber, pageSize);
        }

        public Task<IQueryable<Product>> GetProductAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Product> PostProductAsync(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}