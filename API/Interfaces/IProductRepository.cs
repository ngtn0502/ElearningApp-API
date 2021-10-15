using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void Update(Products product);


        Task<IQueryable<Products>> GetProductsAsync();

        Task<Products> GetProductByIdAsync(int id);

        Task<Products> PostProductAsync(Products product);

        Task<Products> EditProductAsync(int id, Products product);

        Task<DeleteResponse> DeleteProductAsync(int id);

        Task<PageResponse> SearchProductAsync(string query, int? pageNumber, int? pageSize);

        Task<PageResponse> GetCoursesAsync(int category, int? pageNumber, int? pageSize);

    }
}