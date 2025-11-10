using System.Security.Cryptography;
using System.Text;

using URLShortening.Interfaces;

namespace URLShortening.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        public string GenerateShortCode(string url)
        {
            // Simple approach: hash + take first 6 chars
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url + DateTime.Now.Ticks));
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash.Substring(0, 6);
        }
    }
}
