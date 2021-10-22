using System.Collections.Generic;
using System.Threading.Tasks;
using API.ApiViewModels;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product product);


        Task<IEnumerable<ProductDTOs>> GetProductsAsync();

        ProductDTOs GetProductByIdAsync(int id);

        Task<ProductDTOs> CreateProductAsync(ProductViewModels product);

        Task<ProductDTOs> UpdateProductAsync(int id, ProductViewModels product);

        Task<DeleteResponse> DeleteProductAsync(int id);

        Task<PagedResponse> SearchProductAsync(string query, int? pageNumber, int? pageSize);

        Task<PagedResponse> GetAllProductsAsync(int category, int? pageNumber, int? pageSize);

    }
}