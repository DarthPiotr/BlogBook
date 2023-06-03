using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogBook.ViewModel
{
    public class Register
    {
        [Required(ErrorMessage = "Nazwa użytkownnika jest wymagana")]
        [DataType(DataType.Text)]
        [DisplayName("Nazwa użytkownika")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DisplayName("Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Proszę powtórzyć hasło")]
        [DisplayName("Potwierdź hasło")]
        [Compare(nameof(Password), ErrorMessage = "Hasła nie są zgodne")]
        public string ConfirmPassword { get; set; }

    }

}
