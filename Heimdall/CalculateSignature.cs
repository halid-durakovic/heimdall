using System;
using System.Security.Cryptography;
using System.Text;

namespace Heimdall
{
    public interface ICalculateSignature
    {
        string Calculate(string secret, string messageRepresentation);
    }

    public class CalculateSignature : ICalculateSignature
    {
        public string Calculate(string secret, string messageRepresentation)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secret);
            var messageRepresentationBytes = Encoding.UTF8.GetBytes(messageRepresentation);

            using (var hmac = new HMACSHA256(secretKeyBytes))
            {
                var hash = hmac.ComputeHash(messageRepresentationBytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
