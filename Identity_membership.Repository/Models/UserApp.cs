using Microsoft.AspNetCore.Identity;
using Identity_membership.Core.Models;

namespace Identity_membership.Repository.Models
{
    public class UserApp:IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
