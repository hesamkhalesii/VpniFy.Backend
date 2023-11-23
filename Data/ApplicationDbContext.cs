
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VpniFy.Backend.Model;

namespace VpniFy.Backend.Data
{
	public class ApplicationDbContext : IdentityDbContext <User,Role,long>
	{
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
       
    }
}
