using System.ComponentModel.DataAnnotations;

namespace Identity_membership.web.ViewModels
{
    public class SignInVM
    {
        public SignInVM() { }

        public SignInVM(string email, string password, bool rememberMe)
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
        }

        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = null!;

        [Display(Name ="Beni Hatırla")]
        public bool RememberMe { get; set; }

    }

}
