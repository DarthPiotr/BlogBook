using BlogBook.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BlogBook.Pages
{
	/*
     * Zapomnia�em has�a POST/GET>
     *                               ForgotPassword
     * Podanie Email POST>
     *                               SendEmail
     * Odpowied�                     -> ForgotPassword
     * 
     *                      -------------
     * Klikni�cie w link GET>
     *                               ResetPassword/ForgotPassword
     * Podanie nowego has�a POST>   
     *                               NewPassword
     * Odpowied�                     -> Success/ForgotPassword
     * 
     * */


	public class ResetPasswordModel : PageModel
    {
        private UserManager<AppIdentityUser> userManager;
        private SignInManager<AppIdentityUser> signInManager;
        private readonly IEmailSender _sender;

        [BindProperty]
        public ViewModel.ResetPassword ResetModel { get; set; }

        public ResetPasswordModel(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager, IEmailSender sender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._sender = sender;

        }

        public async Task<IActionResult> OnGet(string? Id, string? code, string? returnUrl)
        {
            if (Id == null || code == null) {
				ViewData["reason"] = "ForgotPassword";
				return Page();
			}

			var user = await userManager.FindByIdAsync(Id);
			if (user == null)
            {
				ViewData["messageBad"] = $"Co� posz�o nie tak... Spr�buj jeszcze raz";
				ViewData["reason"] = "ForgotPassword";
				return Page();
            }
                
            ViewData["id"] = Id;
			ViewData["code"] = code;
			ViewData["reason"] = "ResetPassword";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string reason, string? code, string? id)
        {
            ViewData["reason"] = reason;
            if (reason == "SendEmail" && ResetModel.Email != null) {

				var user = await userManager.FindByEmailAsync(ResetModel.Email);
                if (user != null)
                {
				    var token = await userManager.GeneratePasswordResetTokenAsync(user);
				    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var callbackUrl = Url.Page(
					    "/ResetPassword",
					    pageHandler: null,
					    values: new { area = "", user.Id, code },
					    protocol: Request.Scheme);
				    var body = "Zg�oszono ��danie resetu has�a zwi�zanego z tym adresem email. Kliknij w ten <a href=\"" + callbackUrl + "\">link</a>, aby stworzy� nowe has�o.";

				    await _sender.SendEmailAsync(user.Email, "Reset has�a BlogBook", body);

                    ViewData["message"] = $"Na adres <strong>{user.Email}</strong> zosta� wys�any mail z ��daniem zmiany has�a.";
					ViewData["reason"] = "ForgotPassword";
				}
			}
            else if (reason == "NewPassword" && code != null && id != null)
            {
				var user = await userManager.FindByIdAsync(id);
				if (user != null)
				{
                    try
                    {
                        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                        await userManager.ResetPasswordAsync(user, token, ResetModel.Password);

					    ViewData["message"] = $"Has�o dla <strong>{user.Email}</strong> zosta�o zmienione.";
                        ViewData["reason"] = "Success";
                    }
                    catch (Exception e)
                    {
						ViewData["messageBad"] = $"Co� posz�o nie tak... Spr�buj jeszcze raz";
						ViewData["reason"] = "ForgotPassword";
						return Page();
					}
				}
			}

            return Page();
        }
    }
}
