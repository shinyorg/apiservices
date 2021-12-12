﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shiny.Api.Push.Providers.Impl
{
    public class FcmResponse
    {
        [JsonPropertyName("multicast_id")]
        public string MulticastId { get; set; }

        [JsonPropertyName("canonical_ids")]
        public int CanonicalIds { get; set; }

        /// <summary>
        /// Success count
        /// </summary>
        public int Success { get; set; }

        /// <summary>
        /// Failure count
        /// </summary>
        public int Failure { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public List<FcmResult> Results { get; set; }

        ///// <summary>
        ///// Returns value indicating notification sent success or failure
        ///// </summary>
        //public bool IsSuccess()
        //{
        //    return Success > 0 && Failure == 0;
        //}
    }


    public class FcmResult
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("registration_id")]
        public string RegistrationId { get; set; }

        public string Error { get; set; }
    }
}

