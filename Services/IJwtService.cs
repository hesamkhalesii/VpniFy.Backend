using System.Threading.Tasks;
using VpniFy.Backend.Model;

namespace VpniFy.Backend.Services
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}