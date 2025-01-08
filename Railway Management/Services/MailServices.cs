using System.Net.Mail;
using System.Net;

namespace Railway_Management.Services
{
    public class MailServices : IMailService
    {
        int success = 0;
        string smtpHost = "smtp.gmail.com";
        int smtpPort; //
        string smtpUser = string.Empty;
       string smtpPass = string.Empty;

        private readonly IConfiguration _configuration;
        public MailServices(IConfiguration configuration)
        {
            _configuration = configuration;
            smtpPass = _configuration["MyAppSetting:MailKey"].ToString();
            smtpUser = _configuration["MyAppSetting:Email"].ToString();
            smtpPort = int.Parse(_configuration["MyAppSetting:smtpPort"]);
            
        }

       

        int IMailService.SendForgotPassworOTPMail(string generateUrl, string mailId)
        {
            string toEmail = mailId;
            string subject = "Your One Time Generate Link for Forgot Password";
            string body = $@"<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; }}
        .container {{ width: 80%; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); }}
        .header {{ background-color: #2E8B57; color: white; padding: 10px 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .footer {{ background-color: #2E8B57; color: white; padding: 10px 20px; text-align: center; border-radius: 0 0 8px 8px; font-size: 12px; }}
        h2 {{ color: #2E8B57; }}
        a {{ color: #2E8B57; text-decoration: none; }}
        .content {{ margin-top: 20px; }}
        .otp-container {{ background-color: #f1f1f1; padding: 15px; text-align: center; font-size: 30px; font-weight: bold; margin: 20px 0; border-radius: 5px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>RailwayManagement</h1>
        </div>
        <div class='content'>
            <h2>Hello {mailId},</h2>
            <p>We received a request to  Forgotting Password. Please use the following Link to complete your transaction:</p>
            <div class=""otp-container"">
                <p>{generateUrl}</p>
            </div>
            <p>This Link is valid for the next 20 minutes. If you did not request this, please ignore this email.</p>
            <p>For more assistance, feel free to reach out to us at <a href=""mailto:support@railwaymanagement.com"">support@railwaymanagement.com</a>.</p>
        </div>
        <div class='footer'>
            <p>This is an automated email. Please do not reply to this message directly.</p>
            <p>If you no longer wish to receive these emails, please <a href='unsubscribe-link'>unsubscribe</a>.</p>
        </div>
    </div>
</body>
</html>";



            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };


                MailMessage mailMessage = new MailMessage(smtpUser, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                // Email bhejein
                smtpClient.Send(mailMessage);
                success++;
                return success;



            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        int IMailService.SendOTPMail(string generatedOTP, string mailId,string userName)
        {
            string toEmail =mailId;
            string subject = "Your One-Time Password (OTP) for Verification";
            string body = $@"<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; }}
        .container {{ width: 80%; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); }}
        .header {{ background-color: #2E8B57; color: white; padding: 10px 20px; text-align: center; border-radius: 8px 8px 0 0; }}
        .footer {{ background-color: #2E8B57; color: white; padding: 10px 20px; text-align: center; border-radius: 0 0 8px 8px; font-size: 12px; }}
        h2 {{ color: #2E8B57; }}
        a {{ color: #2E8B57; text-decoration: none; }}
        .content {{ margin-top: 20px; }}
        .otp-container {{ background-color: #f1f1f1; padding: 15px; text-align: center; font-size: 30px; font-weight: bold; margin: 20px 0; border-radius: 5px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>RailwayManagement</h1>
        </div>
        <div class='content'>
            <h2>Hello {userName},</h2>
            <p>We received a request to verify your identity with a One-Time Password (OTP). Please use the following OTP to complete your transaction:</p>
            <div class=""otp-container"">
                <p>{generatedOTP}</p>
            </div>
            <p>This OTP is valid for the next 10 minutes. If you did not request this, please ignore this email.</p>
            <p>For more assistance, feel free to reach out to us at <a href=""mailto:support@railwaymanagement.com"">support@railwaymanagement.com</a>.</p>
        </div>
        <div class='footer'>
            <p>This is an automated email. Please do not reply to this message directly.</p>
            <p>If you no longer wish to receive these emails, please <a href='unsubscribe-link'>unsubscribe</a>.</p>
        </div>
    </div>
</body>
</html>";



            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };


                MailMessage mailMessage = new MailMessage(smtpUser, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                // Email bhejein
                smtpClient.Send(mailMessage);
                success++;
                return success;



            }
            catch(Exception ex)
            {
                return 0;
            }

        }

        int IMailService.SendSuccessMail(string RecipendName, string mailId)
        {
            
            

          
            string toEmail = mailId; 
            string subject = "Thank You for Choosing RailwayManagement Services";

            
            string body = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; }}
                .container {{ width: 80%; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); }}
                .header {{ background-color: #2E8B57; color: white; padding: 10px 20px; text-align: center; border-radius: 8px 8px 0 0; }}
                .footer {{ background-color: #2E8B57; color: white; padding: 10px 20px; text-align: center; border-radius: 0 0 8px 8px; font-size: 12px; }}
                h2 {{ color: #2E8B57; }}
                a {{ color: #2E8B57; text-decoration: none; }}
                .content {{ margin-top: 20px; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>RailwayManagement</h1>
                </div>
                <div class='content'>
                    <h2>Hello {RecipendName} ,</h2>
                    <p>Thank you for using RailwayManagement Services. We are pleased to inform you that your recent transaction has been successfully processed. We appreciate your trust in our services and are committed to providing you with the best experience possible.</p>
                    <p><strong>What’s Next?</strong></p>
                    <ul>
                        <li>Keep an eye on your account for upcoming updates.</li>
                        <li>For more details, feel free to check our <a href='https://www.railwaymanagement.com'>website</a>.</li>
                        <li>If you have any questions or need assistance, our support team is here to help. You can reach us at <a href='mailto:support@railwaymanagement.com'>support@railwaymanagement.com</a>.</li>
                    </ul>
                    <p>We look forward to continuing to serve your railway management needs.</p>
                    <p>Best regards,</p>
                    <p><strong>The RailwayManagement Team</strong></p>
                </div>
                <div class='footer'>
                    <p>This is an automated email. Please do not reply to this message directly.</p>
                    <p>If you no longer wish to receive these emails, please <a href='unsubscribe-link'>unsubscribe</a>.</p>
                </div>
            </div>
        </body>
        </html>";

            try
            {
               
                SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true 
                };

               
                MailMessage mailMessage = new MailMessage(smtpUser, toEmail, subject, body)
                {
                    IsBodyHtml = true 
                };

                // Email bhejein
                smtpClient.Send(mailMessage);
                success++;
                return success;
            }
            catch (Exception ex)
            {
                return success;
                
            }
        }


    }
}
