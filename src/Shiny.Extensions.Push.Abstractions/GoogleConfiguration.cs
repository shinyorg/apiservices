using System;


namespace Shiny.Extensions.Push
{
    public class GoogleConfiguration
    {
        public string SenderId { get; set; }
        public string ServerKey { get; set; }
        public string? DefaultChannelId { get; set; }

        public void AssertValid()
        {
            ArgumentNullException.ThrowIfNull(this.SenderId);
            ArgumentNullException.ThrowIfNull(this.ServerKey);
        }
    }
}
