using EmailEngine.Contracts;
using EmailEngine.Helpers;
using SendGrid;
using SendGrid.Helpers.Mail;
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
        private List<SendGrid.Helpers.Mail.Attachment> attachments = null;
        public SendGridEmailService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void SetApiKey(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void SetAttachments(List<SendGrid.Helpers.Mail.Attachment> attachments)
        {
            this.attachments = attachments;
        }

        public string SendMail(string subject, string body, string from, string fromName,
            string recipient, string cc, string bcc, bool isBodyHtml = false)
        {
            var message = "";
            try
            {
                Execute(subject, body, from, fromName, recipient, cc, bcc, isBodyHtml).Wait(1000);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
            
        }

        private async System.Threading.Tasks.Task<string> Execute(string subject, string body, string from, string fromName,
            string recipient, string cc, string bcc,  bool isBodyHtml = false)
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
                    var client = new SendGridClient(apiKey);
                    var msg = Compose(subject, body, from, fromName, recipient, cc, bcc, attachments, isBodyHtml);
                    var response = await client.SendEmailAsync(msg);
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        throw new Exception("Bad Request");
                    }
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


        private SendGridMessage Compose(string subject, string body, string from, string fromName,
            string to, string cc, string bcc, List<SendGrid.Helpers.Mail.Attachment> attachments, bool isBodyHtml = false)
        {

           
            var plainTextContent = isBodyHtml ? "" : body;
            var htmlContent = !isBodyHtml ? "" : body;

            var fromList = new EmailAddress(from, fromName);
            var toList = new List<EmailAddress>();
            var subjects = new List<string>();
            subjects.Add(subject);
           
            string[] toArray = null;
            if (to.Contains(';'))
            {
                toArray = to.Split(';');
            }
            else
            {
                toArray = new string[] { to };
            }

            foreach (var toEmail in toArray)
            {
                if (toEmail.Trim() != string.Empty)
                {
                    //msg.AddTo(new EmailAddress(toEmail));
                    toList.Add(new EmailAddress(toEmail));
                }
            }

            var msg = MailHelper.CreateSingleEmail(fromList, toList.First(), subject, plainTextContent, htmlContent);

            if (!string.IsNullOrEmpty(cc))
            {
                string[] ccArray = null;
                if (cc.Contains(';'))
                {
                    ccArray = cc.Split(';');
                }
                else
                {
                    ccArray = new string[] { cc };
                }

                foreach (var ccEmail in ccArray)
                {
                    if (ccEmail.Trim() != string.Empty)
                    {
                        msg.AddCc(new EmailAddress(ccEmail));
                    }
                }
            }

            if (!string.IsNullOrEmpty(bcc))
            {
                string[] bccArray = null;
                if (bcc.Contains(';'))
                {
                    bccArray = bcc.Split(';');
                }
                else
                {
                    bccArray = new string[] { bcc };
                }

                foreach (var bccEmail in bccArray)
                {
                    if (bccEmail.Trim() != string.Empty)
                    {
                        msg.AddCc(new EmailAddress(bccEmail));
                    }
                }
            }

            if (attachments != null && attachments.Count() > 0)
                msg.AddAttachments(attachments);


            return msg;
        }
    }
}
