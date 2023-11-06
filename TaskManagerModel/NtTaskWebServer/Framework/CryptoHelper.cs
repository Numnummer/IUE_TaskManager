using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework
{
    public static class CryptoHelper
    {
        public static string HashString(string input)
        {
            var algorithm = new SHA256Managed();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input+"sаlt");
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            string hashedValue = BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
            return hashedValue;
        }
    }
}
