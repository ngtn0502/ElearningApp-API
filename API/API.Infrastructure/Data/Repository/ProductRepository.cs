using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.ApiViewModels;
using AutoMapper;
using System.Collections.Generic;
using API.Helpers;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        async Task<IEnumerable<ProductDTOs>> IProductRepository.GetProductsAsync()
        {
            var products = await _context.Products.Include(s => s.Detail).ToListAsync();

            var productsToReturn = _mapper.Map<IEnumerable<ProductDTOs>>(products);

            return productsToReturn;
        }

        ProductDTOs IProductRepository.GetProductByIdAsync(int id)
        {
            var product = _context.Products.Include(s => s.Detail).ToList().Find(el => el.Id == id);

            var productToReturn = _mapper.Map<ProductDTOs>(product);

            return productToReturn;
        }


        void IProductRepository.Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        async Task<ProductDTOs> IProductRepository.CreateProductAsync(ProductViewModels newProduct)
        {

            Product product = new Product();

            product.Name = newProduct.Name;
            product.Description = newProduct.Description;
            product.Rating = newProduct.Rating;
            product.Price = newProduct.Price;
            product.CategoryId = newProduct.CategoryId;
            product.ImageUrl = newProduct.ImageUrl;
            product.Instructor = newProduct.Instructor;
            product.Language = newProduct.Language;


            await _context.Products.AddAsync(product);
            _context.SaveChanges();


            ProductDetail productDetail = new ProductDetail();
            productDetail.Detail = newProduct.Detail;
            productDetail.ProductId = product.Id;

            await _context.ProductDetail.AddAsync(productDetail);
            _context.SaveChanges();

            _context.Products.Include(s => s.Detail).ToList().Find(el => el.Id == product.Id);

            var productsToReturn = _mapper.Map<ProductDTOs>(product);

            return productsToReturn;
        }

        async Task<ProductDTOs> IProductRepository.UpdateProductAsync(int id, ProductViewModels newProduct)
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

            _context.SaveChanges();

            var productDetail = await _context.ProductDetail.FindAsync(id);

            productDetail.Detail = newProduct.Detail;
            _context.SaveChanges();

            var productsToReturn = _mapper.Map<ProductDTOs>(product);

            return productsToReturn;
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

        async Task<PagedResponse> IProductRepository.SearchProductAsync(string query, int? pageNumber, int? pageSize)
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
                                  select product).Include(s => s.Detail).ToListAsync();

            var productsToReturn = _mapper.Map<List<ProductDTOs>>(products);

            return PagedList.CreatePagedResponse(productsToReturn, pageNumber, pageSize);
        }

        async Task<PagedResponse> IProductRepository.GetAllProductsAsync(int category, int? pageNumber, int? pageSize)
        {

            var products = await _context.Products.Include(s => s.Detail).ToListAsync();

            if (category != 0)
            {
                products = await _context.Products.Where(x => x.CategoryId == category).Include(s => s.Detail).ToListAsync();
            }

            var productsToReturn = _mapper.Map<List<ProductDTOs>>(products);

            return PagedList.CreatePagedResponse(productsToReturn, pageNumber, pageSize);
        }

        public Task<ProductViewModels> CreateProductAsync(ProductViewModels product)
        {
            throw new System.NotImplementedException();
        }
    }
}