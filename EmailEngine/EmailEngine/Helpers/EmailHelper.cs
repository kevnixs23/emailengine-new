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
    public static class EmailHelper
    {
        public static void ParseSubjectAndBody(Dictionary<string, string> bodyValues, ref string subject, ref string body)
        {
            try
            {
                if (bodyValues != null)
                {
                    foreach (var key in bodyValues.Keys)
                    {
                        body = body.Replace(string.Format("<<{0}>>", key), bodyValues[key] ?? string.Empty);
                        subject = subject.Replace(string.Format("<<{0}>>", key), bodyValues[key] ?? string.Empty);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
