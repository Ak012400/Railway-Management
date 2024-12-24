using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Railway_Management.Models
{
    public class Password
    {
        

        public static bool PasswordValidation(string password,string hassedPassword)
        {
            return VerifyHashPassword(password, hassedPassword);
        }
       
        public static string GetHassedPassword(string password)
        {
           return  CreateHash(password);
        }

        private static string CreateHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private static bool VerifyHashPassword(string password, string storedHash)
        {
            string hashedPassword = CreateHash(password);
            return hashedPassword == storedHash;
        }

        public static string NewPasswordValidation(string password,string newPassword)
        {
            return NewPasswordCreateValidatoin(password, newPassword);

        }
        private static string NewPasswordCreateValidatoin(string password,string confirmPassword)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()])[A-Za-z\d!@#$%^&*()]{8,}$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(password))
            {
                if(password == confirmPassword)
                {
                    return "done";
                }
                else
                {
                    return "Passwords are not matching...";
                }
            }
            else
            {
                return "Password should have all the combination and also with 8 charector length";
            }



           // return regex.IsMatch(password);
        }







    }
}
