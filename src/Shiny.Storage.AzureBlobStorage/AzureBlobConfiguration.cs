namespace Shiny.Storage.AzureBlobStorage
{
    public record AzureBlobConfiguration(
        string ConnectionString, 
        string BlobContainerName,
        string BlobName
    );
}
