using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SampleWeb.Contracts;
using Shiny;
using System.Windows.Input;


namespace SampleMobile.Mail
{
    public class CreateMailViewModel : ViewModel
    {
        public CreateMailViewModel(AppSettings app)
        {
            var valid = this.WhenAny(
                x => x.TemplateName,
                x => x.ToAddress,
                x => x.Subject,
                (template, to, subj) =>
                {
                    if (template.GetValue().IsEmpty())
                        return false;

                    if (to.GetValue().IsEmpty())
                        return false;

                    if (subj.GetValue().IsEmpty())
                        return false;

                    return true;
                }
            );

            this.Send = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    await app.ApiClient.SendMail(this.TemplateName, this.CreateMsg());
                    await this.Dialogs.Snackbar("Sent E-Mail Successfully");
                },
                valid
            );

            this.Parse = ReactiveCommand.CreateFromTask(
                async () =>
                {
                    var response = await app.ApiClient.TestMailParse(this.TemplateName, this.CreateMsg());
                    await this.Dialogs.Alert(response);
                },
                valid
            );
        }


        public ICommand Send { get; }
        public ICommand Parse { get; }

        [Reactive] public string Subject { get; set; } = "Test Subject";
        [Reactive] public string Body { get; set; } = "Hello World";

        [Reactive] public string TemplateName { get; set; } = "test";
        [Reactive] public bool IsHighPriority { get; set; }

        [Reactive] public string ToAddress { get; set; }
        [Reactive] public string ReplyToAddress { get; set; }


        SendMail CreateMsg() => new SendMail
        {
            To = this.ToAddress,
            Subject = this.Subject,
            ReplyTo = this.ReplyToAddress,
            AdditionalMessage = this.Body,
            IsHighPriority = this.IsHighPriority
        };
    }
}
