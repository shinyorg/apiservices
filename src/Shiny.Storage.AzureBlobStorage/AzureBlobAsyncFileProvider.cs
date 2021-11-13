using Azure.Storage.Blobs;


namespace Shiny.Storage.AzureBlobStorage
{
    public class AzureBlobAsyncFileProvider : IAsyncFileProvider
    {
        readonly BlobClient client;


        public AzureBlobAsyncFileProvider()
        {
            //new BlobClient("", "");
        }

        public Task<IDirectory> CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IFile> GetFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}