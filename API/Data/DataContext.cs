using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductDetail> ProductDetail { get; set; }
        public DbSet<Category> Category { get; set; }
        
    }
}