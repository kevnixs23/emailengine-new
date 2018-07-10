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
    public interface ISendGridEmailService
    {
        string SendMail(string subject, string body, string from, string fromName,
               string recipient, string cc, string bcc, bool isBodyHtml = false);
    }
}
