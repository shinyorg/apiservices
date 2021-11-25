using System.Collections.Generic;

namespace SampleWeb.Contracts
{
    public class Notification : Registration
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string>? Data { get; set; }
    }
}
