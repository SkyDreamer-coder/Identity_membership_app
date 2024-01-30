using Identity_membership.web.Models;
using System.ComponentModel.DataAnnotations;

namespace Identity_membership.web.ViewModels
{
    public class UserEditVM
    {
        [Required(ErrorMessage = "Kullanıcı Ad alanı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi :")]
        public DateTime? BirtDate { get; set; }

        [Display(Name = "Şehir :")]
        public string? City { get; set; }

        [Display(Name = "Profil Resmi :")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Cinsiyet :")]
        public Gender? Gender { get; set; }

    }
}
