using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogBook.Pages
{
    public class LogoutModel : PageModel
    {
        private SignInManager<IdentityUser> signInManager;

        [BindProperty]
        public ViewModel.Login Model { get; set; }


        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;

        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }
        public IActionResult OnPostNoLogoutAsync(string returnUrl = "")
        {
            if (String.IsNullOrEmpty(returnUrl) || returnUrl == "/")
            {
                return RedirectToPage("Index");
            }
            else
            {
                return RedirectToPage(returnUrl);
            }
        }
    }
}
