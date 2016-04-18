using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shop.Domain.Base;
using Shop.Repositories.Context;
using Shop.RepositoriesFacade.Base;

namespace Shop.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity, new()
    {
        private readonly IDatabaseContext context;

        public Repository(IDatabaseContext context)
        {
            this.context = context;
        }

        protected DbSet<T> Set
        {
            get { return context.Set<T>(); }
        }

        #region Sync (Delete, DeleteAll, Create, CreateAll, Update, UpdateAll)
        public virtual T Delete(int id)
        {
            var entity = new T { Id = id };
            Set.Attach(entity);
            return Set.Remove(entity);
        }

        public virtual IEnumerable<T> DeleteAll(IEnumerable<int> ids)
        {
            var entities = ids.Select(id => new T { Id = id }).Select(entity => Set.Attach(entity));
            return Set.RemoveRange(entities);
        }

        public virtual T Create(T entity)
        {
            return Set.Add(entity);
        }

        public virtual IEnumerable<T> CreateAll(IEnumerable<T> entities)
        {
            return Set.AddRange(entities);
        }

        public virtual void Update(T entity)
        {
            var local = Set.Local.FirstOrDefault(x => x.Id == entity.Id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }

            Set.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateAll(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }
        #endregion

        #region Async (Count, Get, GetAll, GetPage)
        public virtual async Task<int> CountAsync()
        {
            return await Set.AsNoTracking().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Set.AsNoTracking().CountAsync(predicate);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await Set.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Set.AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await Set.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Set.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<IList<T>> GetPageAsync<TOrder>(int skip, int take, Expression<Func<T, TOrder>> orderBy, bool desc, Expression<Func<T, bool>> predicate)
        {
            var query = from e in Set select e;
            if (predicate != null)
            {
                query = Set.AsNoTracking().Where(predicate);
            }

            query = desc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query.Skip(skip).Take(take).ToListAsync();
        }
        #endregion

        #region Sync (Get)
        public T Get(int id)
        {
            return Set.AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Set.AsNoTracking().SingleOrDefault(predicate);
        }
        #endregion

        #region Save changes
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
        #endregion
    }
}