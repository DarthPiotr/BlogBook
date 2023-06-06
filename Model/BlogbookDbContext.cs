using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogBook.Model;

namespace BlogBook.Model
{
    public class BlogbookDbContext : IdentityDbContext
    {
        public BlogbookDbContext(DbContextOptions<BlogbookDbContext> options) : base(options)
        {

        }
        public DbSet<BlogBook.Model.Post> Post { get; set; } = default!;
        public DbSet<BlogBook.Model.Like> Like { get; set; } = default!;
    }
}
