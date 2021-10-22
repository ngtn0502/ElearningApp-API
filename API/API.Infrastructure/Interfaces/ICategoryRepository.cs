using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        void Update(Category category);

        Task<IQueryable<Category>> GetCategoryAsync();

        Task<Category> GetCategoryByIdAsync(int id);

        Task<Category> CreateCategoryAsync(Category category);
    }
}