using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product product);


        Task<IQueryable<Product>> GetProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task<Product> PostProductAsync(Product product);

        Task<Product> EditProductAsync(int id, Product product);

        Task<DeleteResponse> DeleteProductAsync(int id);

        Task<PageResponse> SearchProductAsync(string query, int? pageNumber, int? pageSize);

        Task<PageResponse> GetCoursesAsync(int category, int? pageNumber, int? pageSize);

    }
}