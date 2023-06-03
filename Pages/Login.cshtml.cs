using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogBook.Pages
{
    public class LoginModel : PageModel
    {
		private SignInManager<IdentityUser> signInManager;

		[BindProperty]
        public ViewModel.Login Model { get; set; }


		public LoginModel(SignInManager<IdentityUser> signInManager)
		{
			this.signInManager = signInManager;

		}

		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync(string ReturnUrl = "")
		{

			if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false);
				if (identityResult.Succeeded)
				{
					if (String.IsNullOrEmpty(ReturnUrl) || ReturnUrl == "/")
					{
						return RedirectToPage("Index");
					}
					else
					{
						return RedirectToPage(ReturnUrl);
					}
				}

				ModelState.AddModelError("", "Nieprawid³owy email lub has³o");
			}

			return Page();
		}
	}
}
