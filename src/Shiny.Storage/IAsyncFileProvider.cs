using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Storage
{
    public interface IAsyncFileProvider
    {
        Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default);
        Task<IFile> GetFile(string path, CancellationToken cancellationToken = default);
        Task<IDirectory> CreateDirectory(string path, CancellationToken cancellationToken = default);

        // TODO: file system watcher? possible for ftp or azure blobs?

        //Task Delete(string path);
        //Task MoveTo(string sourcePath, string destinationPath);
        //Task CopyTo(string sourcePath, string destrinationPath);
    }
}