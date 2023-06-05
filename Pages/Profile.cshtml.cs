using BlogBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlogBook.Pages
{
	public class ProfileModel : PageModel
	{
		public List<Post> Posts { get; set; }
		public IdentityUser ProfileUser { get; set; }

		private readonly BlogbookDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public ProfileModel(BlogbookDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> OnGet(string? userName, string? query)
		{
			if (string.IsNullOrEmpty(userName))
			{
				// Gdy nie podano parametru, przekieruj na aktualnego u¿ytkownika
				var currentUser = await _userManager.GetUserAsync(User);
				if (currentUser == null)
				{
					return RedirectToPage("Index");
				}
				return RedirectToPage("Profile", new { userName = currentUser.UserName });
			}

			ProfileUser = await _userManager.FindByNameAsync(userName);
			if (ProfileUser == null)
			{
				// Gdy podany u¿ytkownik nie istnieje, przekieruj na stronê domow¹
				return RedirectToPage("Index");
			}

			if (!string.IsNullOrEmpty(query))
			{
				var filteredContext = _context.Post
				.Where(p => p.User.Id == ProfileUser.Id)
					.Where(p => EF.Functions.Like(p.Content, $"%{query}%") ||
								EF.Functions.Like(p.Title, $"%{query}%") ||
								EF.Functions.Like(p.User.UserName, $"%{query}%"))
				.Include(p => p.User)
				.OrderByDescending(p => p.PostDate);
				Posts = await filteredContext.ToListAsync();

				return Page();
			}

			var blogbookDbContext = _context.Post
				.Where(p => p.User.Id == ProfileUser.Id)
				.Include(p => p.User)
				.OrderByDescending(p => p.PostDate);
			Posts = await blogbookDbContext.ToListAsync();
			return Page();
		}
	}
}
