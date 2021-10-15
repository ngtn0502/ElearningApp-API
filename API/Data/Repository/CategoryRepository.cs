using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        void ICategoryRepository.Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;

        }

        async Task<IQueryable<Category>> ICategoryRepository.GetCategoryAsync()
        {
            var categories = await _context.Category.Include(a => a.Products).ToListAsync();
            return categories.AsQueryable();
        }

        async Task<Category> ICategoryRepository.GetCategoryByIdAsync(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return null;
            return category;

        }

        async Task<Category> ICategoryRepository.PostCategoryAsync(Category category)
        {
            await _context.Category.AddAsync(category);
            return category;
        }

        public Task<IQueryable<Category>> GetCategoryAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}