using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;


namespace Shiny.Storage.AzureBlobStorage
{
    public class AzureBlobAsyncFileProvider : IAsyncFileProvider
    {
        readonly Lazy<BlobClient> client;


        public AzureBlobAsyncFileProvider(IOptions<AzureBlobConfiguration> options)
        {
            this.client = new Lazy<BlobClient>(() => new BlobClient(
                options.Value.ConnectionString,
                options.Value.BlobContainerName,
                options.Value.BlobName
            ));
        }

        public Task<IDirectory> CreateDirectory(string path)
        {
            //this.client.OpenReadAsync(new Azure.Storage.Blobs.Models.BlobOpenReadOptions()
            //{

            //});
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