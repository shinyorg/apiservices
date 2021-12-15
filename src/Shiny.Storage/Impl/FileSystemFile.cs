namespace Shiny.Storage.Impl
{
    public class FileSystemFile : IFile
    {
        readonly FileInfo file;
        public FileSystemFile(FileInfo file) => this.file = file;


        public long Size => this.file.Length;
        public string Name => this.file.Name;
        public string FullName => this.file.FullName;
        public bool Exists => this.file.Exists;
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
