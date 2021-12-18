using System;

namespace Shiny.Storage.AzureBlobStorage
{
    public class AzureBlobDirectory : IDirectory
    {
        public string Name => throw new NotImplementedException();

        public string FullName => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public DateTimeOffset? LastAccessTime => throw new NotImplementedException();

        public DateTimeOffset? LastWriteTime => throw new NotImplementedException();

        public DateTimeOffset? CreationTime => throw new NotImplementedException();
    }
}
