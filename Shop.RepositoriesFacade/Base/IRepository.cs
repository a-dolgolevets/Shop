using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shop.Domain.Base;

namespace Shop.RepositoriesFacade.Base
{
    public interface IRepository<T> : IDisposable where T : IBaseEntity
    {
        #region Sync (Delete, DeleteAll, Create, CreateAll, Update, UpdateAll)
        T Delete(int id);

        IEnumerable<T> DeleteAll(IEnumerable<int> ids);

        T Create(T entity);

        IEnumerable<T> CreateAll(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateAll(IEnumerable<T> entities);
        #endregion

        #region Async (Count, Get, GetAll, GetPage)
        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(int id);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetAllAsync();

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetPageAsync<TOrder>(int skip, int take, Expression<Func<T, TOrder>> orderBy, bool desc, Expression<Func<T, bool>> predicate);
        #endregion

        #region Sync (Get)
        T Get(int id);

        T Get(Expression<Func<T, bool>> predicate);
        #endregion

        #region Save changes

        Task<int> SaveChangesAsync();

        int SaveChanges();

        #endregion
    }
}