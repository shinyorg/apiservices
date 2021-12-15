namespace Shiny.Storage
{
    public interface IAsyncFileProvider
    {
        Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default);
        Task<IFile> GetFile(string path);

        Task<IDirectory> CreateDirectory(string path);

        // TODO: file system watcher? possible for ftp or azure blobs?

        //Exists(string path)
        //Delete(string path)
        //MoveTo(string source, string destination)
        //CopyTo(string source, string destrination)
    }
}