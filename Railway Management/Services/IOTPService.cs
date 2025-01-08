namespace Railway_Management.Services
{
    public interface IOTPService
    {
        bool OTPValidation(string username, string inputOTP);
        string OTPGenerationForUser(string username);
    }
}
