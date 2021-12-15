using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;


//https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet#:~:text=Azure%20Blob%20Storage%20is%20Microsoft%27s%20object%20storage%20solution,Blob%20Storage%20client%20library%20v12%20for%20.NET%20to%3A
namespace Shiny.Storage.AzureBlobStorage
{
    public class AzureBlobAsyncFileProvider : IAsyncFileProvider
    {
        readonly Lazy<BlobClient> client;


        public AzureBlobAsyncFileProvider(IOptions<AzureBlobConfiguration> options)
        {
            //new BlobContainerClient("", "").Create
            this.client = new Lazy<BlobClient>(() => new BlobClient(
                options.Value.ConnectionString,
                options.Value.BlobContainerName,
                options.Value.BlobName
            ));
        }

        public Task<IDirectory> CreateDirectory(string path)
        {
            //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
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