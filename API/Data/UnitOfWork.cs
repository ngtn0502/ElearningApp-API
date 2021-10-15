using System.Threading.Tasks;
using API.Data.Repository;
using API.Interfaces;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        IProductRepository IUnitOfWork.ProductRepository => new ProductRepository(_context);

        ICategoryRepository IUnitOfWork.CategoryRepository => new CategoryRepository(_context);

        async Task<bool> IUnitOfWork.Complete()
        {
            return await _context.SaveChangesAsync() > 1;
        }

        bool IUnitOfWork.HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}