using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shop.Domain.Base;
using Shop.ServicesFacade.Helpers;

namespace Shop.ServicesFacade.Base
{
    public interface IService<T> : IDisposable where T : IBaseEntity
    {
        #region Write (Delete, DeleteAll, Create, CreateAll, Update, UpdateAll)
        Task<OperationResult> DeleteAsync(int id);

        Task<OperationResult> DeleteAllAsync(IEnumerable<int> ids);

        OperationResult<T> Create(T entity);

        OperationResult CreateAll(IEnumerable<T> entities);

        Task<OperationResult<T>> CreateAsync(T entity);

        Task<OperationResult> CreateAllAsync(IEnumerable<T> entities);

        Task<OperationResult<T>> UpdateAsync(T entity);

        Task<OperationResult> UpdateAllAsync(IEnumerable<T> entities);
        #endregion

        #region Read (Count, Get, GetAll, GetPage)
        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        T Get(int id);

        T Get(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(int id);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetAllAsync();

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetPageAsync<TOrder>(int skip, int take, Expression<Func<T, TOrder>> orderBy, bool desc, Expression<Func<T, bool>> predicate = null);
        #endregion 
    }
}