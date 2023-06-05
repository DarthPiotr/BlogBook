using BlogBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogBook.Pages
{
    public class ProfileModel : PageModel
    {
        public List<Post> Posts { get; set; }

        private readonly BlogbookDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileModel(BlogbookDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string? userName)
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

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                // Gdy podany u¿ytkownik nie istnieje, przekieruj na stronê domow¹
                return RedirectToPage("Index");
            }

            var blogbookDbContext = _context.Post
                .Where(p => p.User.Id == user.Id)
                .Include(p => p.User);
            Posts = await blogbookDbContext.ToListAsync();
            return Page();
        }
    }
}
