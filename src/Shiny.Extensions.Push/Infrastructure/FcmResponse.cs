using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class FcmResponse
    {
        [JsonPropertyName("multicast_id")]
        public long MulticastId { get; set; }

        [JsonPropertyName("canonical_ids")]
        public long CanonicalIds { get; set; }

        public int Success { get; set; }
        public int Failure { get; set; }
        public List<FcmResult>? Results { get; set; }
    }


    public class FcmResult
    {
        [JsonPropertyName("message_id")]
        public string? MessageId { get; set; }

        [JsonPropertyName("registration_id")]
        public string? RegistrationId { get; set; }

        public string? Error { get; set; }
    }
}

