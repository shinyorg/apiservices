using System;

namespace Shiny.Extensions.Push.Infrastructure
{
    public class NoSendException : Exception
    {
        public static readonly NoSendException Instance = new NoSendException();
    }
}
