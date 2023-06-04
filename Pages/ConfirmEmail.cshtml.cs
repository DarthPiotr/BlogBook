using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BlogBook.Pages
{
    public class ConfirmEmailModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        // https://localhost:7275/ConfirmEmail?
        // area=Identity&
        // userId=a27370f8-4998-4201-8ea5-dbfdf8f87bc2&
        // code=Q2ZESjhBZWg3SzBZWWo1SnFpVHFCWC8xV3d0NmxvdTZSMGU0NWlrOVJrR0daL2wzYnp5WmpRSWw4WmEweGh3Q2tPb2lJN1liZTh6WmhhYzMzeUVKejgySTNzS3N3SnF1MitJN2V5aStrR0VzbFJVOEkwVGRJaGNWcmRoRHdUa1h0WkppVFk5WVlUVk8vdG9ybnpnZCsxVWgyWk1iLzBGcGN6QU5Ma0Y1VGFRdmk1ZGh2bnR1ZnQ0c3NQbEhJbXNPM08waXNLUU1oUTF0bWdFdVBLT2NGcDEvWXcvMkFrd1ZOSlVYbENwRHpjZHFuK1Ntb0gwK05MNGdFbWZlQUk0bXJJU05VQT09&
        // returnUrl=%2F

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet(string? userId, string? code, string? returnUrl)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("Login");
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
