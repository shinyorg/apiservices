//using System.Collections.Generic;
//using Newtonsoft.Json;


//namespace Shiny.Api.Push.Providers
//{
////{
////    "aps" : {
////        "alert" : {
////            "loc-key" : "GAME_PLAY_REQUEST_FORMAT",
////            "loc-args" : [ "Jenna", "Frank"]
////},
////        "sound" : "chime.aiff"
////    },
////    "acme" : "foo"
////}
//    class AppleNotification : Dictionary<string, string>
//    {
//        public class Alert
//        {
//            [JsonProperty("title")]
//            public string? Title { get; set; }

//            [JsonProperty("body")]
//            public string? Body { get; set; }

//            [JsonProperty("badge")]
//            public int? Badge { get; set; }

//            [JsonProperty("sound")]
//            public string? Sound { get; set; }
//        }

//        [JsonProperty("content-available")]
//        public int ContentAvailable { get; set; } = 1;

//        [JsonProperty("alert")]
//        public Alert? AlertBody { get; set; }



//        //[JsonProperty("apns-push-type")]
//        //public string PushType { get; set; } = "alert";
//    }
//}
