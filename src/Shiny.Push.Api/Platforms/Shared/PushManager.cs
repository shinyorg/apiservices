#if !NETSTANDARD
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Push.Infrastructure;


namespace Shiny.Push
{
    public sealed class PushManager : IPushManager, IPushTagSupport, IShinyStartupTask
    {
        readonly PushContainer container;
        readonly INativeAdapter adapter;
        readonly ILogger logger;
        readonly PushConfiguration config;


        public PushManager(PushContainer container,
                           INativeAdapter adapter,
                           PushConfiguration config,
                           ILogger<PushManager> logger)
        {
            this.adapter = adapter;
            this.container = container;
            this.logger = logger;
            this.config = config;

            this.config.Http ??= new HttpClient();
        }


        public DateTime? CurrentRegistrationTokenDate => this.container.CurrentRegistrationTokenDate;
        public string? CurrentRegistrationToken => this.container.CurrentRegistrationToken;
        public string[]? RegisteredTags => this.container.RegisteredTags;


        public async void Start()
        {
            this.adapter.OnTokenRefreshed = async token =>
            {
                this.container.SetCurrentToken(token, false);
                await this.container.OnTokenRefreshed(token).ConfigureAwait(false);

                // TODO: call httpclient
            };

            this.adapter.OnReceived = push => this.container.OnReceived(push);
            this.adapter.OnEntry = push => this.container.OnEntry(push);

            await this.container
                .TryAutoStart(this.adapter, this.logger)
                .ConfigureAwait(false);
        }


        public IObservable<PushNotification> WhenReceived() => this.container.WhenReceived();


        public async Task<PushAccessState> RequestAccess(CancellationToken cancelToken = default)
        {
            var result = await this.adapter.RequestAccess().ConfigureAwait(false);
            this.container.SetCurrentToken(result.RegistrationToken!, false);
            return new PushAccessState(result.Status, this.CurrentRegistrationToken);
        }


        public async Task UnRegister()
        {
            await this.adapter.UnRegister().ConfigureAwait(false);
            this.container.ClearRegistration();
        }

        public Task SetTags(params string[]? tags)
        {
            throw new NotImplementedException();
        }

        public Task AddTag(string tag)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTag(string tag)
        {
            throw new NotImplementedException();
        }

        public Task ClearTags()
        {
            throw new NotImplementedException();
        }


        //public async Task AddTag(string tag)
        //{
        //    var tags = this.container.RegisteredTags?.ToList() ?? new List<string>(1);
        //    tags.Add(tag);

        //    await FirebaseMessaging.Instance.SubscribeToTopic(tag);
        //    this.container.RegisteredTags = tags.ToArray();
        //}


        //public async Task RemoveTag(string tag)
        //{
        //    var list = this.container.RegisteredTags?.ToList() ?? new List<string>(0);
        //    if (list.Remove(tag))
        //        this.container.RegisteredTags = list.ToArray();

        //    await FirebaseMessaging
        //        .Instance
        //        .UnsubscribeFromTopic(tag)
        //        .AsAsync()
        //        .ConfigureAwait(false);
        //}


        //public async Task ClearTags()
        //{
        //    if (this.container.RegisteredTags != null)
        //    {
        //        foreach (var tag in this.container.RegisteredTags)
        //        {
        //            await FirebaseMessaging
        //                .Instance
        //                .UnsubscribeFromTopic(tag)
        //                .AsAsync()
        //                .ConfigureAwait(false);
        //        }
        //    }
        //    this.container.RegisteredTags = null;
        //}


        //public async Task SetTags(params string[]? tags)
        //{
        //    await this.ClearTags().ConfigureAwait(false);
        //    if (tags != null)
        //    {
        //        foreach (var tag in tags)
        //            await this.AddTag(tag).ConfigureAwait(false);
        //    }
        //}
    }
}
#endif