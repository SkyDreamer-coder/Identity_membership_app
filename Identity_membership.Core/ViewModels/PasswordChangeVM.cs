using System.ComponentModel.DataAnnotations;

namespace Identity_membership.Core.ViewModels
{
    public class PasswordChangeVM
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Eski şifre alanı boş bırakılamaz.")]
        [Display(Name = "Eski şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordEx { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordNew { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifre aynı değildir.")]
        [Required(ErrorMessage = "Yeni şifre tekrar alanı boş bırakılamaz.")]
        [Display(Name = "Yeni şifre tekrar :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string PasswordNewConfirm { get; set; } = null!;

        public PasswordChangeVM() { }

        public PasswordChangeVM(string passwordEx, string passwordNew, string passwordNewConfim)
        {
            PasswordEx = passwordEx;
            PasswordNew = passwordNew;
            PasswordNewConfirm = passwordNewConfim;
        }

    }
}
