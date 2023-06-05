using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogBook.Pages
{
    public class LoginModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

		[BindProperty]
        public ViewModel.Login Model { get; set; }


		public LoginModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;

		}

		public IActionResult OnGet(string email = "")
        {
			if (!String.IsNullOrEmpty(email))
			{
				ViewData["Message"] = $"Na adres <strong>{email}</strong> zosta³ wys³any mail z linkiem do aktywacji konta.";
			}
            return Page();
        }

		public async Task<IActionResult> OnPostAsync(string ReturnUrl = "")
		{
            // Require the user to have a confirmed email before they can log on.
            var user = await userManager.FindByEmailAsync(Model.Email);
            if (user != null)
            {
                if (!await userManager.IsEmailConfirmedAsync(user))
                {
					ViewData["Message"] = "Proszê potwierdziæ adres email przed logowaniem.";
                    return Page();
                }
            }

            if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(user?.UserName ?? "", Model.Password, Model.RememberMe, false);
				if (identityResult.Succeeded)
				{
					if (String.IsNullOrEmpty(ReturnUrl) || ReturnUrl == "/")
					{
						return RedirectToPage("Index");
					}
					else
					{
						return Redirect(ReturnUrl);
					}
				}

				ModelState.AddModelError("", "Nieprawid³owy email lub has³o");
			}

			return Page();
		}
	}
}
