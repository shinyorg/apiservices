namespace Shiny.Storage.Impl
{
    public class FileSystemDirectory : IDirectory
    {
        public FileSystemDirectory(DirectoryInfo directory)
        {
            directory.
        }

        public string Name => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public DateTimeOffset? LastAccessed => throw new NotImplementedException();

        public DateTimeOffset? CreationTime => throw new NotImplementedException();
    }
}
