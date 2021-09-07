using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;


namespace Shiny.Api.Push.Providers
{
    public class GooglePushProvider : IPushProvider
    {
        public GooglePushProvider()
        {
            //var creds = GoogleCredential.FromJson(JsonConvert.SerializeObject(new TestGoogleCredential
            //{
            //    ProjectId = Secrets.Values.GoogleCredentialProjectId,
            //    PrivateKeyId = Secrets.Values.GoogleCredentialPrivateKeyId,
            //    PrivateKey = Secrets.Values.GoogleCredentialPrivateKey,
            //    ClientId = Secrets.Values.GoogleCredentialClientId,
            //    ClientEmail = Secrets.Values.GoogleCredentialClientEmail,
            //    ClientCertUrl = Secrets.Values.GoogleCredentialClientCertUrl
            //}));
            //FirebaseApp.Create(new AppOptions
            //{
            //    Credential = creds
            //});
        }


        public async Task Send(Notification notification)
        {
            //var msg = new Message
            //{
            //    Token = this.pushManager.CurrentRegistrationToken,
            //    Data = new Dictionary<string, string>
            //    {
            //        { "stamp", stamp }
            //    }
            //};
            //await FirebaseMessaging.DefaultInstance.SendAsync(msg);
            throw new System.NotImplementedException();
        }
    }
}
