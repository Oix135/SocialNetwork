using System.Security;

namespace SocialNetwork.BLL.Models
{
    public class UserAuthenticationData
    {
        public string Email { get; set; }
        public SecureString Password { get; set; }
    }
}
