using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApiAuthentication
{
    public interface ICalculateSignature
    {
        string Generate(string secretKey, string messageRepresentation);
    }

    public class CalculateSignature : ICalculateSignature
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
