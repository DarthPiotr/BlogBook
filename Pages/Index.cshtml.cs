using BlogBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogBook.Pages
{
    public class IndexModel : PageModel
    {

		private UserManager<IdentityUser> userManager;
		private SignInManager<IdentityUser> signInManager;
		private readonly BlogbookDbContext _context;

		[BindProperty]
		public List<Post> Model { get; set; }


		public IndexModel(BlogbookDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this._context = context;
		}

		public async Task<IActionResult> OnGet()
        {
			var blogbookDbContext = _context.Post.Include(p => p.User)
				.OrderByDescending(p => p.Likes)
				.ThenByDescending(p => p.PostDate)
				.Take(5);

			Model = await blogbookDbContext.ToListAsync();

			return Page();
		}
    }
}