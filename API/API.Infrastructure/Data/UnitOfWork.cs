using System.Threading.Tasks;
using API.Data.Repository;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        IProductRepository IUnitOfWork.ProductRepository => new ProductRepository(_context, _mapper);

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