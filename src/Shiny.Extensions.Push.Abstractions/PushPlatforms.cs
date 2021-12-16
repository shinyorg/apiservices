using System;

namespace Shiny.Extensions.Push
{
    [Flags]
    public enum PushPlatforms
    {
        Apple = 1,
        Google = 2,

        All = Apple | Google
    }
}
