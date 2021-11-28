using System;

namespace Shiny.Api.Push
{
    [Flags]
    public enum PushPlatforms
    {
        Apple = 1,
        Google = 2,

        All = Apple | Google
    }
}
