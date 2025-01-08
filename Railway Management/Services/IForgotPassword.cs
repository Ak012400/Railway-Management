namespace Railway_Management.Services
{
    public interface IForgotPassword
    {
        int ForgotUserPassword(string useremail, string password);
        bool VerifyTokenForForgotPassword(string token, string userEmail);
        string getGeneratedToken(string useremail);


    }
}
