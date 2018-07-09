using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailEngine.Contracts
{
    public interface IEmailService 
    {
        string SendMail(string subject, string body,
            string recipient, string emailFromAddress, string emailFromName, string cc, string bcc, Attachment[] attachments, bool isBodyHtml = false);
    }
}
