using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.Models
{
    public class UserApp:IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
