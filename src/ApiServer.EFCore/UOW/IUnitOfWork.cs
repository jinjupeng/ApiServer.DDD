using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace ApiServer.EFCore.UOW
{
    public interface IUnitOfWork
    {
        DbContext GetDbContext();
        IDbContextTransaction Begin();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Complete(IDbContextTransaction transaction);

        Task CompleteAsync(IDbContextTransaction transaction);

        void RollBackChanges();

        void Dispose();
    }
}
