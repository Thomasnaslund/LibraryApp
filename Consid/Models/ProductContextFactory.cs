using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace Consid.Models
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //Connection string for scaffolding and ef migrations, might not be needed on Visual studio Windows
            optionsBuilder.UseSqlServer("data source =.\\SQLEXPRESS; initial catalog = lib_db; Integrated Security = True;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}