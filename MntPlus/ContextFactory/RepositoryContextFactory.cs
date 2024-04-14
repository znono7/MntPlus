using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace MntPlus.WPF
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        private const string DefaultConnectionString = "Data Source=C:\\Users\\nour-\\source\\repos\\MntPlus\\MntPlusDatabase.db";

        public RepositoryContext CreateDbContext(string[] args)
        {
           
            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                                .UseSqlite(DefaultConnectionString,
                                              b => b.MigrationsAssembly("MntPlus.WPF"));
            return new RepositoryContext(builder.Options);

        }
    }
}
