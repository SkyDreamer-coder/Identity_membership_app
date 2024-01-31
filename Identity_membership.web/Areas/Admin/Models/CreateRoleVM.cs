using System.ComponentModel.DataAnnotations;

namespace Identity_membership.web.Areas.Admin.Models
{
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Role adı alanı boş bırakılamaz.")]
        [Display(Name = "Role adı :")]
        public string Name { get; set; }
    }
}
