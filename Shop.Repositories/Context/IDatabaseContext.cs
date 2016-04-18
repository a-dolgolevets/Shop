using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Shop.Domain.Base;

namespace Shop.Repositories.Context
{
    public interface IDatabaseContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class, IBaseEntity;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbEntityEntry Entry(object entity);
    }
}