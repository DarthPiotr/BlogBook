using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BlogBook.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
		private readonly IEmailSender _sender;

		[BindProperty]
        public ViewModel.Register Model { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender sender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
			this._sender = sender;

		}

        public async Task<IActionResult> OnGet()
        {
			return Page();
		}

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
        {

            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = Model.Username,
                    Email = Model.Email
                };

                var result = await userManager.CreateAsync(user, Model.Password);

                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);


					var userId = await userManager.GetUserIdAsync(user);
					var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

					var callbackUrl = Url.Page(
						"/ConfirmEmail",
						pageHandler: null,
						values: new { area = "", userId, code, returnUrl },
						protocol: Request.Scheme);

                    var body = "PotwierdŸ rejestracjê klikaj¹c w ten <a href=\"" + callbackUrl + "\">link</a>";

                    await _sender.SendEmailAsync(user.Email, "Weryfikacja BlogBook", body);

					return RedirectToPage("Login", new { email=user.Email });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
    }
}
