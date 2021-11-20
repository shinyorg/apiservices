using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public class GoogleResult
    {
        //[JsonProperty("message_id")]
        public string MessageId { get; set; }

        //[JsonProperty("registration_id")]
        public string RegistrationId { get; set; }

        public string Error { get; set; }
    }
}
