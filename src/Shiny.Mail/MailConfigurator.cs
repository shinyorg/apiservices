namespace Shiny.Mail
{
    public class MailConfigurator
    {
        public MailConfigurator UseSmtpSender()
        {
            return this;
        }


        public MailConfigurator AddSender<TSender>() where TSender : IMailSender
        {
            return this;
        }
    }
}
