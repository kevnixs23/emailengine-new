using EmailEngine.Contracts;
using EmailEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailEngine.Services
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private string apiKey = "";

        public SendGridEmailService(string apiKey){
            this.apiKey = apiKey;
        }

        public async System.Threading.Tasks.Task<string> SendMail(string subject, string body, string from, string fromName,
            string recipient, string cc, string bcc, List<SendGrid.Helpers.Mail.Attachment> attachments, bool isBodyHtml = false)
        {
            int portNo = 0;
            bool useSSL = false;

            var EmailFromAddress = from;
            var EmailFromName = fromName;

            string message = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(recipient))
                {
                    message = "Unable to send email. Client email address not available.";
                }
                else if (string.IsNullOrEmpty(body))
                {
                    message = "Unable to send email. Email body is empty.";
                }
                else if (string.IsNullOrEmpty(subject))
                {
                    message = "Unable to send email. Email subject is empty.";
                }
                else
                {
                    await EmailSender.ExecuteViaSendGrid(
                         apiKey,
                         recipient,
                         EmailFromAddress,
                         EmailFromName,
                         subject,
                         body,
                         cc,
                         bcc,
                         isBodyHtml,
                         useSSL,
                         attachments);
                }
            }
            catch (SmtpException smtpEx)
            {
                if (smtpEx.InnerException != null)
                {
                    message = smtpEx.InnerException.Message;
                }
                else
                {
                    message = smtpEx.Message;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }
    }
}
