using System;
using System.IO;
using System.Threading.Tasks;


namespace Shiny.Storage.Impl
{
    public class FileSystemFileInfo : IFileInfo
    {
        FileInfo? file;
        DirectoryInfo? directory;


        public FileSystemFileInfo(string path)
        {
            if (Utils.IsDirectory(path))
                this.directory = new DirectoryInfo(path);
            else
                this.file = new FileInfo(path);
        }


        public long? Size => this.file?.Length;
        public string Name => (this.file?.Name ?? this.directory?.Name)!;
        public string Path => (this.file?.FullName ?? this.directory?.FullName)!;
        public bool Exists => this.file?.Exists ?? this.directory?.Exists ?? false;
        public bool IsDirectory => this.directory != null;
        public DateTimeOffset? LastAccessTime => this.file?.LastAccessTimeUtc ?? this.directory?.LastAccessTimeUtc;
        public DateTimeOffset? LastWriteTime => this.file?.LastWriteTimeUtc ?? this.directory?.LastWriteTimeUtc;
        public DateTimeOffset? CreationTime => this.file?.CreationTimeUtc ?? this.directory?.CreationTimeUtc;


        public Task<Stream> OpenStream(bool forWrite)
        {
            if (this.directory != null)
                throw new ArgumentException("You can't open a stream for a directory");

            var stream = forWrite ? File.OpenWrite(this.file.FullName) : File.OpenRead(this.file.FullName);
            return Task.FromResult<Stream>(stream);
        }
    }
}
