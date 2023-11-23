using System.ComponentModel.DataAnnotations;

namespace VpniFy.Backend.Model
{
    public class TokenRequest
    {

        public string username { get; set; }
        public string password { get; set; }
        //public string refresh_token { get; set; }
        //public string scope { get; set; }

        //public string client_id { get; set; }
        //public string client_secret { get; set; }
    }
}
