using System.Text.RegularExpressions;

namespace UserAPI.Infrastructure
{
    public class DataChecker
    {
        private const string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        public static bool IsEmail(string email)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
