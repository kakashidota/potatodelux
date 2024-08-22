using DockerCompose.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerCompose.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
