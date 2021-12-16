using System.Net.Mail;

namespace Shiny.Extensions.Mail.Impl
{
    public class SmtpMailSender : IMailSender
    {
        readonly SmtpConfig config;
        public SmtpMailSender(SmtpConfig config)
            => this.config = config ?? throw new ArgumentNullException(nameof(config));



        public async Task Send(MailMessage mail)
        {
            using (var smtp = new SmtpClient())
            {
                if (this.config.Host != null)
                    smtp.Host = this.config.Host;

                if (this.config.PickupDirectoryLocation != null)
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = this.config.PickupDirectoryLocation;
                }
                if (this.config.Port != null)
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = this.config.Port.Value;
                }
                smtp.EnableSsl = this.config.EnableSsl;

                await smtp
                    .SendMailAsync(mail)
                    .ConfigureAwait(false);
            }
        }
    }
}
