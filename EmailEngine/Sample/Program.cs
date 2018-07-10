using EmailEngine.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var SMTPServer = ConfigurationManager.AppSettings["SMTPServer"] ?? string.Empty;
            var SMTPPortNo = ConfigurationManager.AppSettings["SMTPPortNo"] ?? string.Empty;
            var SMTPUsername = ConfigurationManager.AppSettings["SMTPUsername"] ?? string.Empty;
            var SMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"] ?? string.Empty;
            var SMTPUseSSL = ConfigurationManager.AppSettings["SMTPUseSSL"];
            var SendGridApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];

            var fromEmail = "developerPH129@gmail.com";
            var from = "developer";
            var subject = "";
            var body = "";
            var to = "";
            var cc = "";
            var bcc = "";
            var option = "";
            //var sendEmailService = new EmailService(SMTPServer, SMTPPortNo, SMTPUsername, SMTPPassword, SMTPUseSSL);
            //var sendEmailService = new EmailService();
            var sendGridEmailService = new SendGridEmailService(SendGridApiKey);
            while (true)
            {
                Console.WriteLine("Please input subject");
                subject = Console.ReadLine();
                Console.WriteLine("Please input recipient");
                to = Console.ReadLine();
                Console.WriteLine("Please input cc");
                cc = Console.ReadLine();
                Console.WriteLine("Please input bcc");
                bcc = Console.ReadLine();
                Console.WriteLine("Please input body");
                body = Console.ReadLine();
                var message = "";

                //var message = sendEmailService.SendMail(subject,
                //                           body,
                //                           to,
                //                           fromEmail,
                //                           from,
                //                           cc,
                //                           bcc,
                //                           null,
                //                           false);

                //var message = await sendGridEmailService.SendMail(subject,
                //               body,
                //               to,
                //               fromEmail,
                //               from,
                //               cc,
                //               bcc,
                //               null,
                //               false);

                if (string.IsNullOrEmpty(message))
                    Console.WriteLine("Successfully sent");
                else
                {
                    Console.WriteLine("Failed!!");
                    Console.WriteLine(message);
                }
                Console.WriteLine("Press X to send another email");
                option = Console.ReadLine();
                if (option.ToLower().Trim() == "x")
                {
                    subject = "";
                    body = "";
                    to = "";
                    cc = "";
                    bcc = "";
                    option = "";
                }
                else
                    break;
            }


        }
    }
}
