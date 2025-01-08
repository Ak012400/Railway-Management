namespace Railway_Management.Services
{
    public interface IMailService
    {
        int SendSuccessMail(string RecipendName, String mailId);
        int SendOTPMail(string generatedOTP, String mailId,string userName);
        int SendForgotPassworOTPMail(string forgotURL, string mailId);



    }
}
