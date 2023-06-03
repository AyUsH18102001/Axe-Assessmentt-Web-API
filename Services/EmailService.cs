using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace AxeAssessmentToolWebAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        
        public EmailService(IConfiguration configuration, DataContext context) 
        { 
            this._dataContext = context;
            this._configuration = configuration;
        }

        public async Task<string> SendTokenEmail(int usedId,string testName)
        {
            // find the user
            User? user = await _dataContext.Users.FindAsync(usedId);
            if (user == null)
            {
                return "false";
            }
            StringBuilder content = new StringBuilder();
            content.AppendLine($"Dear {user.FirstName} <br>");
            content.AppendLine("Congratulations on shortlisting of your resume. It lloks like we have a candidate of utmost calliber <br>");
            content.AppendLine("We will be requesting you to take an assessment test on the AXE ASSESSMENT TOOL for us to see your logical and problem solving skills <br><br>");

            string emailHTML = $"<html>" + $"<body> " + $"{content} <br> " + $"Please visit " + $"<a href=\"http://localhost:4200/tokenLogin\">Axe Assessment Tool</a> and proceed with your test <br><br>" + $"Please use the code <b>{user.UserToken}</b>" + $"" +
                $"</body></html>";
            // send the email
            try
            {
                string fromMail = _configuration.GetSection("EmailSettings:clientEmail").Value;
                string fromPassword = _configuration.GetSection(key: "EmailSettings:clientPassword").Value;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = $"Axe Finance Invite for Test | {testName} Test";
                message.To.Add(new MailAddress(user.Email));
                message.Body = emailHTML;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                };

                smtpClient.Send(message);
                return "success";
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }
    }
}
