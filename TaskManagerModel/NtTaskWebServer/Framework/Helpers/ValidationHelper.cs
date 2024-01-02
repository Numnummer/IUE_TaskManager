using HtmlAgilityPack;
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
        public const int MinPasswordLength = 4;
        public const int CodeLength = 6;
        public static bool IsValidLoginData(LoginData loginData)
        {
            var isUserNameValid = loginData.UserName != string.Empty;
            var isEmailValid = IsValidEmail(loginData.Email);
            var isLoginValid = loginData.Login != string.Empty;
            var isPasswordValid = loginData.Password.Length >= MinPasswordLength;
            return isUserNameValid && isEmailValid && isLoginValid && isPasswordValid;
        }

        public static bool IsValidLoginDataForEnter(LoginData loginData)
        {
            var isUserNameValid = loginData.UserName != string.Empty;
            var isPasswordValid = loginData.Password.Length >= MinPasswordLength;
            return isUserNameValid && isPasswordValid;
        }

        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public static bool IsValidTaskData(TaskData? taskData)
            => taskData!=null
                && taskData.Name.Length>0
                && uint.TryParse(taskData.Priority, out uint _)
                && taskData.Deadline > DateTimeOffset.UtcNow;

        public static bool IsValidCodeData(CodeData? codeData)
            => codeData!=null
                && string.IsNullOrWhiteSpace(codeData.Code)
                && string.IsNullOrWhiteSpace(codeData.UserName)
                && codeData.Code.Length == CodeLength
                && int.TryParse(codeData.Code, out var _);
    }
}
