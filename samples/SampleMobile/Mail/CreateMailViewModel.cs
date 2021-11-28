using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;
using System.Windows.Input;


namespace SampleMobile.Mail
{
    public class CreateMailViewModel : ViewModel
    {
        public CreateMailViewModel(ISampleApi api, IDialogs dialogs)
        {
            this.Send = ReactiveCommand.CreateFromTask(
                async () =>
                {

                },
                this.WhenAny(
                    x => x.TemplateName,
                    x => x.FromAddress,
                    x => x.ReplyToAddress,
                    x => x.ToAddress,
                    (template, from, replyTo, to) =>
                    {
                        if (template.GetValue().IsEmpty())
                            return false;

                        if (from.GetValue().IsEmpty())
                            return false;

                        if (replyTo.GetValue().IsEmpty())
                            return false;

                        if (to.GetValue().IsEmpty())
                            return false;

                        return true;
                    } 
                )
            );
        }


        public ICommand Send { get; }

        [Reactive] public string TemplateName { get; set; }
        [Reactive] public bool IsHighPriority { get; set; }

        [Reactive] public string FromDisplayName { get; set; }
        [Reactive] public string FromAddress { get; set; }

        [Reactive] public string ToDisplayName { get; set; }
        [Reactive] public string ToAddress { get; set; }

        [Reactive] public string ReplyToDisplayName { get; set; }
        [Reactive] public string ReplyToAddress { get; set; }
    }
}
