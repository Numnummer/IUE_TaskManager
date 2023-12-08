using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class CryptoHelper
    {
        public static string HashString(string input)
        {
            var encodedInput = Encoding.UTF8.GetBytes(input);
            var salt = GetSalt();
            var iterationCount = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(encodedInput, salt, iterationCount, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }

        private static byte[] GetSalt()
        {
            byte[] salt = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }
    }
}
