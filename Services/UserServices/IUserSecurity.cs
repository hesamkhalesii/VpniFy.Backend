using VpniFy.Backend.Common;
using VpniFy.Backend.Utilities;

namespace VpniFy.Backend.Services.UserServices
{
    public interface IUserSecurity
    {
        long? UserId { get; }
        public bool HasRole(string roleName);
    }
    public class UserSecurity : IUserSecurity, IScopedDependency
    {
        private readonly IHttpContextAccessor _accessor;
        public UserSecurity(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public long? UserId
        {
            get
            {

                var identity = _accessor?.HttpContext?.User.Identity.GetUserId<long>();
                if (identity != null)
                    return identity;

                return null;
            }
        }

        public bool HasRole(string roleName)
        {
            return _accessor.HttpContext?.User.IsInRole(roleName) ?? false;
        }
    }
}
