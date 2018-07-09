using EmailEngine.Helpers;
using System;
using System.Configuration;
using System.Net.Mail;

namespace EmailEngine.Services
{
    public class EmailService
    {
        private string smtpServer { get; set; }
        private string smtpPortNo { get; set; }
        private string smtpUsername { get; set; }
        private string smtpPassword { get; set; }
        private string smtpUseSSL { get; set; }

        //public EmailService()
        //{
        //    smtpServer = ConfigurationManager.AppSettings["SMTPServer"] ?? string.Empty;
        //    smtpPortNo = ConfigurationManager.AppSettings["SMTPPortNo"] ?? string.Empty;
        //    smtpUsername = ConfigurationManager.AppSettings["SMTPUsername"] ?? string.Empty;
        //    smtpPassword = ConfigurationManager.AppSettings["SMTPPassword"] ?? string.Empty;
        //    smtpUseSSL = ConfigurationManager.AppSettings["SMTPUseSSL"] ?? string.Empty;
        //}

        public EmailService(string smtpServer, string smtpPortNo, string smtpUsername, string smptpPassword, string smtpUseSSL) {
            this.smtpServer = smtpServer;
            this.smtpPortNo = smtpPortNo;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
            this.smtpUseSSL = smtpUseSSL;
        }

        public string SendMail(string subject, string body,
            string recipient, string emailFromAddress, string emailFromName, string cc, string bcc, Attachment[] attachments, bool isBodyHtml = false)
        {
            int portNo = 0;
            bool useSSL = false;

            var SMTPServer = smtpServer ?? string.Empty;
            var SMTPPortNo = int.TryParse(smtpPortNo ?? string.Empty, out portNo);
            var SMTPUsername = smtpUsername ?? string.Empty;
            var SMTPPassword = smtpPassword ?? string.Empty;
            var SMTPUseSSL = bool.TryParse(smtpUseSSL ?? string.Empty, out useSSL);
            var EmailFromAddress = emailFromAddress;
            var EmailFromName = emailFromName;

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
                    EmailSender.Execute(
                        SMTPServer,
                        portNo,
                        SMTPUsername,
                        SMTPPassword,
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
