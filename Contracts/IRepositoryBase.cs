﻿using System.Linq.Expressions;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Bulk operations

        void DeleteBulk(IEnumerable<T> entities);
        void CreateBulk(IEnumerable<T> entities);
        void UpdateBulk(IEnumerable<T> entities);


    }
}
