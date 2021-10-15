using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        async Task<IQueryable<Products>> IProductRepository.GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.AsQueryable();
        }

        async Task<Products> IProductRepository.GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }


        void IProductRepository.Update(Products product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        async Task<Products> IProductRepository.PostProductAsync(Products product)
        {
            await _context.Products.AddAsync(product);
            return product;
        }

        async Task<Products> IProductRepository.EditProductAsync(int id, Products newProduct)
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
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 3;

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

            var productsPage = products
                  .Skip((currentPageNumber - 1) * currentpageSize)
                  .Take(currentpageSize).AsQueryable();

            var response = new PageResponse
            {
                PageNumber = currentPageNumber,
                TotalRecords = products.Count,
                Products = productsPage
            };
            return response;
        }

        async Task<PageResponse> IProductRepository.GetCoursesAsync(int category, int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 3;

            var products = await _context.Products.ToListAsync();

            if (category != 0)
            {
                products = await _context.Products.Where(x => x.CategoryId == category).ToListAsync();
            }

            var productsPage = products
            .Skip((currentPageNumber - 1) * currentpageSize)
            .Take(currentpageSize).AsQueryable();

            var response = new PageResponse
            {
                PageNumber = currentPageNumber,
                TotalRecords = products.Count,
                Products = productsPage
            };

            return response;
        }


    }
}