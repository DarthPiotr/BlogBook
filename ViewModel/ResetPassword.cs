using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogBook.ViewModel
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [DisplayName("Adres email, na który zostanie wysłany link do zmiany hasła.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Hasło jest wymagane")]
		[DisplayName("Nowe hasło")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Proszę powtórzyć hasło")]
		[DisplayName("Potwierdź hasło")]
		[Compare(nameof(Password), ErrorMessage = "Hasła nie są zgodne")]
		public string ConfirmPassword { get; set; }
	}
}
