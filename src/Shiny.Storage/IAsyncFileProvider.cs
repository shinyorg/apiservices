namespace Shiny.Storage
{
    public interface IAsyncFileProvider
    {
        Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default);
        Task<IFile> GetFile(string path);

        Task<IDirectory> CreateDirectory(string path);

        // TODO: file system watcher? possible for ftp or azure blobs?

        //Task Delete(string path);
        //Task MoveTo(string sourcePath, string destinationPath);
        //Task CopyTo(string sourcePath, string destrinationPath);
    }
}