using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApiAuthentication
{
    public class CalculateSignature
    {
        public string Generate(string secretKey, string messageRepresentation)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageRepresentationBytes = Encoding.UTF8.GetBytes(messageRepresentation);

            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                var hash = hmac.ComputeHash(messageRepresentationBytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
