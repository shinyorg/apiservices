using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SampleWeb.Contracts;

using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SampleMobile.Storage
{
    public class ExplorerViewModel : ViewModel
    {
        public ExplorerViewModel(ISampleApi api, IDialogs dialogs)
        {
            this.Load = this.LoadingCommand(async () =>
            {
                this.List = await api.GetFileList(new ListStorage
                {
                    ProviderName = this.SelectedProvider,
                    Path = this.SelectedItem.FullName
                });
            });

            this.LoadProviders = ReactiveCommand.CreateFromTask(async () =>
            {
                this.Providers = await api.GetFileProviders();
                this.SelectedProvider = this.Providers.First();
            });

            this.WhenAnyValue(x => x.SelectedProvider)
                .Skip(1)
                .WhereNotNull()
                .Subscribe(_ => this.Load.Execute(null))
                .DisposedBy(this.DestroyWith);
        }


        public override Task InitializeAsync(INavigationParameters parameters)
        {
            this.LoadProviders.Execute(null);
            return base.InitializeAsync(parameters);
        }


        public ICommand Load { get; }
        public ICommand LoadProviders { get; }
        [Reactive] public StorageItem SelectedItem { get; set; }
        [Reactive] public List<StorageItem> List { get; private set; }
        [Reactive] public string SelectedProvider { get; set; }
        [Reactive] public string[] Providers { get; private set; }

    }
}
