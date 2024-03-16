using System.Text;

namespace SocialNetwork.BLL.Managers
{
    internal class PwdEncryptor
    {
        private const string pSalt = "drYv0%6kiF";
        private static string HashSHA256(string text)
        {
            var hash = new StringBuilder();
            byte[] crypto = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(text));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public static string GetPassword(string password)
        {
            return HashSHA256(pSalt + password);
        }
    }
}
