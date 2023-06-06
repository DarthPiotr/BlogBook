using BlogBook.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BlogBook.Pages
{
    public class ConfirmEmailModel : PageModel
    {
        private UserManager<AppIdentityUser> userManager;
        private SignInManager<AppIdentityUser> signInManager;

        public ConfirmEmailModel(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet(string? userId, string? code, string? returnUrl)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("Login", new { returnUrl });
            }
            
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Page();

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, decodedCode);
            if (result.Succeeded)
            {
                ViewData["Success"] = true;
            }

            return Page();
        }
    }
}
