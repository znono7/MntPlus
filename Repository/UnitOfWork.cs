using Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;

        protected RepositoryContext RepositoryContext;

        public UnitOfWork(RepositoryContext repositoryContext) => RepositoryContext = repositoryContext;

        public async Task BeginTransactionAsync()
        {
            _transaction = await RepositoryContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            RepositoryContext.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await RepositoryContext.SaveChangesAsync();
        }
    }
}
