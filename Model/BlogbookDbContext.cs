using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogBook.Model
{
    public class BlogbookDbContext : IdentityDbContext
    {
        public BlogbookDbContext(DbContextOptions<BlogbookDbContext> options) : base(options)
        {

        }
    }
}
