using System.Collections.Generic;
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
        async Task<IEnumerable<Products>> IProductRepository.GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        async Task<Products> IProductRepository.GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        async Task<bool> IProductRepository.SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }


        void IProductRepository.Update(Products product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        async Task<Products> IProductRepository.PostProductAsync(Products product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
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

            await _context.SaveChangesAsync();

            return product;
        }


        async Task<DeleteResponse> IProductRepository.DeleteProductAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(i => i.Id == id);

            if (product == null) return null;

            _context.Remove(product);

            await _context.SaveChangesAsync();

            var returnedObj = new DeleteResponse
            {
                id = product.Id
            };

            return returnedObj;
        }

        async Task<IEnumerable<Products>> IProductRepository.SearchProductAsync(string query)
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
                                  select new Products
                                  {
                                      Id = product.Id,
                                      Name = product.Name,
                                      Description = product.Description,
                                      Rating = product.Rating,
                                      Price = product.Price,
                                      CategoryId = product.CategoryId,
                                      ImageUrl = product.ImageUrl,
                                      Instructor = product.Instructor,
                                      Language = product.Language,
                                  }).ToListAsync();
            return products;
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
            .Take(currentpageSize);

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