using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace UserAPI.Infrastructure
{
    public class DataChecker
    {
        private const string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        private const string patternPassword = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20})";
        public static bool IsEmail(string email)
        {
            Regex regex = new Regex(patternEmail);
            return regex.IsMatch(email);
        }
        public static bool CheckPassword(string password)
        {
            Regex regex = new Regex(patternPassword);
            return regex.IsMatch(password);
        }
    }
}
