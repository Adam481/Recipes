using Recipes.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Recipes.DesignPatterns.Repository
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetItems(Expression<Func<TEntity, bool>> predicate);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }


    /// <summary>
    /// Do not use the class directly outside Data dll. Instead, create instance using dedicated factory.
    /// </summary>
    /// <typeparam name="TEntity">Any entity that exists on db context</typeparam>
    /// <typeparam name="TContext">Entity fraemwork db context</typeparam>
    internal abstract class GenericRepository<TEntity, TContext> : Disposable, IGenericRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext, new()
    {
        protected TContext _dbContext;

        public GenericRepository()
        {
            _dbContext = new TContext();
        }


        public TEntity Get(Guid id)
        {
            Guard.ThrowIfNullOrEmpty(id, "Id cannot be null");

            return _dbContext.Set<TEntity>().Find(id);
        }


        public virtual IEnumerable<TEntity> GetItems(Expression<Func<TEntity, bool>> predicate)
        {
            Guard.ThrowIfNull(predicate, "Predicat cannot be null");

            return _dbContext.Set<TEntity>().Where(predicate).ToList();
        }


        public void Create(TEntity entity)
        {
            Guard.ThrowIfNull(entity, "Entity cannot be null");

            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
        }


        public void Update(TEntity entity)
        {
            Guard.ThrowIfNull(entity, "Entity cannot be null");

            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }


        public void Delete(TEntity entity)
        {
            Guard.ThrowIfNull(entity, "Entity cannot be null");

            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }


        public void DeleteItems(IEnumerable<TEntity> entities)
        {
            Guard.ThrowIfNull(entities, "Entity cannot be null");

            foreach (var entity in entities)
            {
                _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            _dbContext.SaveChanges();
        }


        protected override void DisposeAction()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }
    }
}
