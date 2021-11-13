namespace Shiny.Storage
{
    public interface IAsyncFileProvider
    {
        Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default);
        Task<IFile> GetFile(string path);

        Task<IDirectory> CreateDirectory(string path);
        // TODO: file system watcher?

        //Delete(string path)
        //MoveTo(string dest)
    }
}