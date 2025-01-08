namespace Railway_Management.Services
{
    public class OTPHandler : IOTPService
    {
        private static Dictionary<string, Tuple<string, DateTime>> otpStore = new Dictionary<string, Tuple<string, DateTime>>();
       private string GenerateOTP()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999); 
            return otp.ToString();
        }

        string IOTPService.OTPGenerationForUser(string username)
        {
            string otp = GenerateOTP();
            DateTime expiryTime = DateTime.Now.AddMinutes(5); // OTP valid for 5 minutes
            otpStore[username] = new Tuple<string, DateTime>(otp, expiryTime);
            return otp;
        }

        bool IOTPService.OTPValidation(string userEmail, string otpEntered)
        {
            if (otpStore.ContainsKey(userEmail))
            {
                var storedOTP = otpStore[userEmail];
                if (storedOTP.Item1 == otpEntered && DateTime.Now <= storedOTP.Item2)
                {
                    otpStore.Remove(userEmail); 
                    return true;
                }
                otpStore.Remove(userEmail); 
            }
            return false;
        }
    }
}
