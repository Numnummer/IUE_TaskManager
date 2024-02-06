using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class CryptoHelper
    {
        public static string HashString(string input, string salt)
        {
            var encodedInput = Encoding.UTF8.GetBytes(input);
            var encodedSalt = Encoding.UTF8.GetBytes(salt);
            var iterationCount = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(encodedInput, encodedSalt, iterationCount, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
