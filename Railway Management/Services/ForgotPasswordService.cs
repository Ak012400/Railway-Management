using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Railway_Management.Models;
using System.Data;
using System.Security.Cryptography;


namespace Railway_Management.Services
{
    public class ForgotPasswordService : IForgotPassword
    {
        private static readonly Dictionary<string, (string Token, DateTime Expiry)> _tokens = new();
        private readonly IDbContextFactory<ConnectionContext> _dbContextFactory;
        public ForgotPasswordService(IDbContextFactory<ConnectionContext> contextFactory)
        {
            _dbContextFactory = contextFactory;
        }
        int IForgotPassword.ForgotUserPassword(string useremail, string password)
        {
            password =Password.GetHassedPassword(password);
            try
            {
                int isUpdated = 0;
         
                var email = new SqlParameter("@Email", SqlDbType.VarChar, 255) { Value = useremail };
                var Password = new SqlParameter("@NewPassword", SqlDbType.VarChar, 255) { Value = password };
                using (var dx = _dbContextFactory.CreateDbContext())
                {

                    isUpdated = dx.Database.ExecuteSqlRaw("EXEC ResetPassword @Email, @NewPassword", email, Password);
                }

                return isUpdated;
            }catch
            {
                return 0;
            }
           
        }

        string IForgotPassword.getGeneratedToken(string useremail)
        {
            DateTime expiry = DateTime.Now.AddMinutes(20);
            string token = GenerateTokens();
            if (_tokens.ContainsKey(useremail))
                _tokens[useremail] = (token, expiry); // Overwrite existing token
            else
                _tokens.Add(useremail, (token, expiry));

            return token;
        }

        bool IForgotPassword.VerifyTokenForForgotPassword(string token, string userEmail)
        {
            try
            {
                if (!_tokens.ContainsKey(userEmail)) return false;

                var (storedToken, expiry) = _tokens[userEmail];
                if (storedToken == token && DateTime.Now <= expiry)
                {
                    _tokens.Remove(userEmail); // Invalidate token after use
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        
        private string GenerateTokens()
        {
            var randomBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
