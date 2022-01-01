using System;
using System.IO;
using System.Threading.Tasks;


namespace Shiny.Storage.Impl
{
    public class FileSystemFileInfo : IFileInfo
    {
        readonly FileInfo file;
        public FileSystemFileInfo(string path)
        {

        }


        public long? Size => this.file.Length;
        public string Name => this.file.Name;
        public string Path => this.file.FullName;
        public bool Exists => this.file.Exists;
        public bool IsDirectory => false;
        public DateTimeOffset? LastAccessTime => this.file.LastAccessTimeUtc;
        public DateTimeOffset? LastWriteTime => this.file.LastAccessTimeUtc;
        public DateTimeOffset? CreationTime => this.file.CreationTimeUtc;


        public Task<Stream> OpenStream(bool forWrite)
        {
            var stream = forWrite ? File.OpenWrite(this.file.FullName) : File.OpenRead(this.file.FullName);
            return Task.FromResult<Stream>(stream);
        }
    }
}
