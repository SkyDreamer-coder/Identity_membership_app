using Microsoft.AspNetCore.Identity;

namespace Identity_membership.web.Models
{
    public class UserApp:IdentityUser
    {
        public string City { get; set; }
    }
}
