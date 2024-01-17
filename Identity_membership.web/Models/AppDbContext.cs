using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity_membership.web.Models
{
    public class AppDbContext:IdentityDbContext<UserApp,UserRoleApp,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
            
        
    }
}
