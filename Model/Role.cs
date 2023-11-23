using Microsoft.AspNetCore.Identity;
using VpniFy.Backend.Common;

namespace VpniFy.Backend.Model
{
	public class Role : IdentityRole<long>, IEntity<long>
    {
        public Role()
        {
            
        }
    }
}
