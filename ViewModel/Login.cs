using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BlogBook.ViewModel
{
    public class Login
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DisplayName("Hasło")]
        public string Password { get; set; }


        [DisplayName("Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
