using System.Net.Mail;

namespace Shiny.Mail.Impl
{
    public class SmtpMailSender : IMailSender
    {
        public string? Host { get; set; }
        public int? Port { get; set; }
        public string? PickupDirectoryLocation { get; set; }
        public bool EnableSsl { get; set; } = true;


        public async Task Send(MailMessage mail)
        {
            using (var smtp = new SmtpClient())
            {
                if (this.Host != null)
                    smtp.Host = this.Host;

                if (this.PickupDirectoryLocation != null)
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = this.PickupDirectoryLocation;
                }
                if (this.Port != null)
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = this.Port.Value;
                }
                smtp.EnableSsl = this.EnableSsl;

                await smtp
                    .SendMailAsync(mail)
                    .ConfigureAwait(false);
            }
        }
    }
}
