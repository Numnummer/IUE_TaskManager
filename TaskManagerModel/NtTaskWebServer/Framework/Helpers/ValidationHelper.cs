using NtTaskWebServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NtTaskWebServer.Framework.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidLoginData(LoginData loginData)
        {
            var isUserNameValid = loginData.UserName != string.Empty;
            var isEmailValid = IsValidEmail(loginData.Email);
            var isLoginValid = loginData.Login != string.Empty;
            var isPasswordValid = loginData.Password.Length >= 4;
            return isUserNameValid && isEmailValid && isLoginValid && isPasswordValid;
        }

        public static bool IsValidLoginDataForEnter(LoginData loginData)
        {
            var isUserNameValid = loginData.UserName != string.Empty;
            var isPasswordValid = loginData.Password.Length >= 4;
            return isUserNameValid && isPasswordValid;
        }

        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
