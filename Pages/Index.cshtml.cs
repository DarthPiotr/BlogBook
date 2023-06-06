using BlogBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogBook.Pages
{
    public class IndexModel : PageModel
    {

		private UserManager<AppIdentityUser> userManager;
		private SignInManager<AppIdentityUser> signInManager;
		private readonly BlogbookDbContext _context;

		[BindProperty]
		public List<Post> Model { get; set; }


		public IndexModel(BlogbookDbContext context, UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this._context = context;
		}

		public async Task<IActionResult> OnGet()
        {
			var blogbookDbContext = _context.Post
				.Include(p => p.User)
				.Include(p => p.Likes)
					.ThenInclude(l => l.User)
				.OrderByDescending(p => p.Likes.Count)
				.ThenByDescending(p => p.PostDate)
				.Take(5);

			Model = await blogbookDbContext.ToListAsync();

			return Page();
		}
    }
}