﻿using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext; 

        public RepositoryBase(RepositoryContext repositoryContext) => RepositoryContext = repositoryContext;
        
        public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

        public void CreateBulk(IEnumerable<T> entities) => RepositoryContext.BulkInsert(entities, options => options.IncludeGraph = true);
       

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

        public void DeleteBulk(IEnumerable<T> entities) => RepositoryContext.BulkDelete(entities, options => options.IncludeGraph = true);
       

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? RepositoryContext.Set<T>().AsNoTracking() : RepositoryContext.Set<T>();
       

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges ? RepositoryContext.Set<T>().Where(expression).AsNoTracking() : RepositoryContext.Set<T>().Where(expression);
      

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void UpdateBulk(IEnumerable<T> entities) => RepositoryContext.BulkUpdate(entities, options => options.IncludeGraph = true);
        
    }
}
