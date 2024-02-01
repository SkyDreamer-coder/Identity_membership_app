using System.ComponentModel.DataAnnotations;

namespace Identity_membership.web.Areas.Admin.Models
{
    public class EditRoleVM
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage ="Role adı alanı boş bırakılamaz")]
        [Display(Name ="Role Adı :")]
        public string Name { get; set; } = null!;
    }
}
