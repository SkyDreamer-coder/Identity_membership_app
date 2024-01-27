using System.ComponentModel.DataAnnotations;

namespace Identity_membership.web.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage ="Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage ="Email formatı yanlış")]
        [Display(Name ="Email")]
        public string Email { get; set; }

        public ForgotPasswordVM() { }

        public ForgotPasswordVM(string email)
        {
            Email = email;
        }
    }
}
