namespace Shiny.Storage.Impl
{
    public class FileSystemFile : IFile
    {
        readonly FileInfo file;
        public FileSystemFile(FileInfo file) => this.file = file;


        public long Size => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string FullName => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public DateTimeOffset? LastAccessTime => throw new NotImplementedException();

        public DateTimeOffset? LastWriteTime => throw new NotImplementedException();

        public DateTimeOffset? CreationTime => throw new NotImplementedException();

        public Task<Stream> OpenStream()
        {
            throw new NotImplementedException();
        }
    }
}
