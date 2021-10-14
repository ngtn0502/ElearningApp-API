using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        void Update(Category category);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<Category>> GetCategoryAsync();

        Task<Category> GetCategoryByIdAsync(int id);

        Task<Category> PostCategoryAsync(Category category);
    }
}