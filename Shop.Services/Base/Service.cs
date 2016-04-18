using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shop.Domain.Base;
using Shop.RepositoriesFacade.Base;
using Shop.ServicesFacade.Base;
using Shop.ServicesFacade.Helpers;

namespace Shop.Services.Base
{
    public class Service<T> : IService<T> where T : IBaseEntity
    {
        protected readonly IRepository<T> Repository;

        public Service(IRepository<T> repository)
        {
            Repository = repository;
        }

        #region Write (Delete, DeleteAll, Create, CreateAll, Update, UpdateAll)
        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            Repository.Delete(id);
            return await SaveChangesAsync();
        }

        public virtual async Task<OperationResult> DeleteAllAsync(IEnumerable<int> ids)
        {
            Repository.DeleteAll(ids);
            return await SaveChangesAsync();
        }

        public virtual OperationResult<T> Create(T entity)
        {
            Repository.Create(entity);
            return SaveChanges(entity);
        }

        public virtual OperationResult CreateAll(IEnumerable<T> entities)
        {
            Repository.CreateAll(entities);
            return SaveChanges();
        }

        public virtual async Task<OperationResult<T>> CreateAsync(T entity)
        {
            Repository.Create(entity);
            return await SaveChangesAsync(entity);
        }

        public virtual async Task<OperationResult> CreateAllAsync(IEnumerable<T> entities)
        {
            Repository.CreateAll(entities);
            return await SaveChangesAsync();
        }

        public virtual async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            Repository.Update(entity);
            return await SaveChangesAsync(entity);
        }

        public virtual async Task<OperationResult> UpdateAllAsync(IEnumerable<T> entities)
        {
            Repository.UpdateAll(entities);
            return await SaveChangesAsync();
        }
        #endregion

        #region Read (Count, Get, GetAll, GetPage)
        public virtual async Task<int> CountAsync()
        {
            return await Repository.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Repository.CountAsync(predicate);
        }

        public virtual T Get(int id)
        {
            return Repository.Get(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return Repository.Get(predicate);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await Repository.GetAsync(id);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Repository.GetAsync(predicate);
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public virtual async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Repository.GetAllAsync(predicate);
        }

        public virtual async Task<IList<T>> GetPageAsync<TOrder>(int skip, int take, Expression<Func<T, TOrder>> orderBy, bool desc, Expression<Func<T, bool>> predicate = null)
        {
            return await Repository.GetPageAsync(skip, take, orderBy, desc, predicate);
        }
        #endregion

        #region Save changes
        protected async Task<OperationResult> SaveChangesAsync()
        {
            try
            {
                await Repository.SaveChangesAsync();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        protected async Task<OperationResult<T>> SaveChangesAsync(T updatedEntity)
        {
            try
            {
                await Repository.SaveChangesAsync();
                return new OperationResult<T>(true, updatedEntity);
            }
            catch (Exception ex)
            {
                return new OperationResult<T>(ex, updatedEntity);
            }
        }

        protected OperationResult SaveChanges()
        {
            try
            {
                Repository.SaveChanges();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(ex);
            }
        }

        protected OperationResult<T> SaveChanges(T updatedEntity)
        {
            try
            {
                Repository.SaveChanges();
                return new OperationResult<T>(true, updatedEntity);
            }
            catch (Exception ex)
            {
                return new OperationResult<T>(ex, updatedEntity);
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (Repository != null)
            {
                Repository.Dispose();
            }
        }
        #endregion
    }
}