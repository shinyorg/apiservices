using System;


namespace Shiny.Extensions.Push.Infrastructure
{
    public class ApnException : Exception
    {
        public ApnException(string reason) : base("Apple returned invalid reason: " + reason) { }
    }
}
