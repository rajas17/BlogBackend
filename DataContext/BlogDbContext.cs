using BlogBackend.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.DataContext
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}
