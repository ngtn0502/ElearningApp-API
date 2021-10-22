using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}