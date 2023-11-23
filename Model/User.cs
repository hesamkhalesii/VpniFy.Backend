using Microsoft.AspNetCore.Identity;
using VpniFy.Backend.Common;

namespace VpniFy.Backend.Model
{
	public class User : IdentityUser<long>, IEntity<long>
    {
        public User()
        {
                
        }
    }
}
