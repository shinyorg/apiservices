using System;
using System.IO;
using System.Threading.Tasks;

namespace Shiny.Storage
{
    public interface IFileInfo
    {
        string Name { get; }
        string Path { get; }
        bool Exists { get; }
        bool IsDirectory { get; }

        long? Size { get; }
        Task<Stream> OpenStream(bool forWrite);

        DateTimeOffset? LastAccessTime { get; }
        //DateTimeOffset? LastWriteTime { get; }
        DateTimeOffset? CreationTime { get; }
    }
}
