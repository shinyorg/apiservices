using System.Net.Mail;

namespace Shiny.Mail.Impl
{
    public class SmtpMailSender : IMailSender
    {
        public async Task Send(MailMessage mail)
        {
            using (var smtp = new SmtpClient())
            {
                //smtp.DeliveryFormat = SmtpDeliveryFormat.International
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = "";
                smtp.PickupDirectoryLocation = "";
                smtp.Port = 1;
                smtp.EnableSsl = true;

                await smtp
                    .SendMailAsync(mail)
                    .ConfigureAwait(false);
            }
        }
    }
}
