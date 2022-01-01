using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Storage.Impl
{
    public class FileSystemAsyncFileProvider : IAsyncFileProvider
    {
        public Task<IFileInfo> CreateDirectory(string path, CancellationToken cancellationToken = default)
        {
            var dir = Directory.CreateDirectory(path);
            return Task.FromResult<IFileInfo>(new FileSystemFileInfo(path));
        }


        public Task<IEnumerable<IFileInfo>> GetDirectoryContents(string path, CancellationToken cancelToken = default) => Task.Run<IEnumerable<IFileInfo>>(() =>
        {
            var list = new List<IFileInfo>();
            var entries = Directory
                .EnumerateFileSystemEntries(path)
                .GetEnumerator();

            while (entries.MoveNext() && !cancelToken.IsCancellationRequested)
            {
                var entry = entries.Current;

            }
            return list;
        });


        public Task<IFileInfo> GetFileInfo(string path, CancellationToken cancellationToken = default)
            => Task.FromResult<IFileInfo>(new FileSystemFileInfo(path));


        public Task CopyTo(string sourcePath, string destrinationPath, CancellationToken cancellationToken = default)
        {
            //Directory.Move() or File
            throw new NotImplementedException();
        }


        public Task Delete(string path, CancellationToken cancellationToken = default)
        {
            //Directory or File Delete
            throw new NotImplementedException();
        }


        public Task MoveTo(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
