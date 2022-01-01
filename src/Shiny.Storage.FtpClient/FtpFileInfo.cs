using FluentFTP;

using System;
using System.IO;
using System.Threading.Tasks;


namespace Shiny.Storage.FtpClient
{
    public class FtpFileInfo : IFileInfo
    {
        readonly FtpListItem item;
        public FtpFileInfo(FtpListItem item) => this.item = item;


        public long? Size => this.item.Size;
        public string Name => this.item.Name;
        public string Path => this.item.FullName;

        public bool Exists => throw new NotImplementedException();

        public bool IsDirectory => this.item.Type == FtpFileSystemObjectType.Directory;

        public DateTimeOffset? LastAccessTime => throw new NotImplementedException();

        public DateTimeOffset? LastWriteTime => throw new NotImplementedException();

        public DateTimeOffset? CreationTime => throw new NotImplementedException();

        public Task<Stream> OpenStream(bool forWrite)
        {
            throw new NotImplementedException();
        }
    }
}
