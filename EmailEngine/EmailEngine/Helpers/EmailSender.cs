using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailEngine.Helpers
{
    internal static class EmailSender
    {
        internal static void Execute(string mailServer, int portNo, string smtpUsername, string smtpPassword, string to,
            string from, string fromName, string subject, string body, string cc, string bcc, bool isHtmlEmail, bool useSsl,
            System.Net.Mail.Attachment[] attachments)
        {
            if (string.IsNullOrEmpty(mailServer))
            {
                throw new ArgumentException("'SMTP Server' field cannot be blank.", "SMTP Server");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("'To' field cannot be blank", "Recipient");
            }

            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException("'From' field cannot be blank", "Sender");
            }

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from, fromName);

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
                    mailMessage.To.Add(new MailAddress(toEmail.Trim()));
                }
            }

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
                        mailMessage.CC.Add(ccEmail.Trim());
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
                        mailMessage.Bcc.Add(bccEmail.Trim());
                    }
                }
            }

            mailMessage.Subject = subject;

            if (isHtmlEmail)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);
            }
            else
            {
                AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
                mailMessage.AlternateViews.Add(plainTextView);
            }

            mailMessage.IsBodyHtml = isHtmlEmail;

            SmtpClient smtpClient = new SmtpClient(mailServer)
            {
                EnableSsl = useSsl
            };

            if (portNo != 0) smtpClient.Port = portNo;
            if (!string.IsNullOrEmpty(smtpUsername))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            }
            else
            {
                smtpClient.UseDefaultCredentials = false;
            }

            if ((attachments != null) && (attachments.Any()))
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                mailMessage.Dispose();
                smtpClient.Dispose();
            }
        }

        //public static async Task ExecuteViaSendGrid(string apiKey, string to,
        //    string from, string fromName, string subject, string body, string cc, string bcc, bool isHtmlEmail, bool useSsl,
        //    List<SendGrid.Helpers.Mail.Attachment> attachments)
        //{
        //    if (string.IsNullOrEmpty(apiKey))
        //    {
        //        throw new ArgumentException("SendGrid Api Key cannot be ");
        //    }

        //    if (string.IsNullOrEmpty(to))
        //    {
        //        throw new ArgumentException("'To' field cannot be blank", "Recipient");
        //    }

        //    if (string.IsNullOrEmpty(from))
        //    {
        //        throw new ArgumentException("'From' field cannot be blank", "Sender");
        //    }

        //    var client = new SendGridClient(apiKey);
        //    var plainTextContent = isHtmlEmail ? "" : body;
        //    var htmlContent = !isHtmlEmail ? "" : body;

        //    var fromList = new EmailAddress(from, fromName);
        //    var toList = new List<EmailAddress>();
        //    var subjects = new List<string>();
        //    subjects.Add(subject);
        //    var msg = MailHelper.CreateMultipleEmailsToMultipleRecipients(fromList, toList, subjects, plainTextContent, htmlContent, null);

        //    //var msg = new SendGridMessage()
        //    //{
        //    //    From = new EmailAddress(from, fromName),
        //    //    Subject = subject,
        //    //    PlainTextContent = isHtmlEmail ? "" : body,
        //    //    HtmlContent = isHtmlEmail ? body : ""
        //    //};

        //    string[] toArray = null;
        //    if (to.Contains(';'))
        //    {
        //        toArray = to.Split(';');
        //    }
        //    else
        //    {
        //        toArray = new string[] { to };
        //    }

        //    foreach (var toEmail in toArray)
        //    {
        //        if (toEmail.Trim() != string.Empty)
        //        {
        //            msg.AddTo(new EmailAddress(toEmail));
        //        }
        //    }
           
        //    if (!string.IsNullOrEmpty(cc))
        //    {
        //        string[] ccArray = null;
        //        if (cc.Contains(';'))
        //        {
        //            ccArray = cc.Split(';');
        //        }
        //        else
        //        {
        //            ccArray = new string[] { cc };
        //        }

        //        foreach (var ccEmail in ccArray)
        //        {
        //            if (ccEmail.Trim() != string.Empty)
        //            {
        //                msg.AddCc(new EmailAddress(ccEmail));
        //            }
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(bcc))
        //    {
        //        string[] bccArray = null;
        //        if (bcc.Contains(';'))
        //        {
        //            bccArray = bcc.Split(';');
        //        }
        //        else
        //        {
        //            bccArray = new string[] { bcc };
        //        }

        //        foreach (var bccEmail in bccArray)
        //        {
        //            if (bccEmail.Trim() != string.Empty)
        //            {
        //                msg.AddCc(new EmailAddress(bccEmail));
        //            }
        //        }
        //    }

         
        //    if (attachments != null && attachments.Count() > 0)
        //        msg.AddAttachments(attachments);

          
        //    try
        //    {
        //        var response = await client.SendEmailAsync(msg);
        //        if(response.StatusCode == HttpStatusCode.BadRequest)
        //        {
        //            throw new Exception("Bad Request");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SmtpException(ex.Message);
        //    }
        //}

    }
}
