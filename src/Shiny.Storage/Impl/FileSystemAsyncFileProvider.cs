using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Storage.Impl
{
    public class FileSystemAsyncFileProvider : IAsyncFileProvider
    {
        public Task<IDirectory> CreateDirectory(string path, CancellationToken cancellationToken = default)
        {
            var dir = Directory.CreateDirectory(path);
            return Task.FromResult<IDirectory>(new FileSystemDirectory(dir));
        }


        public Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default) => Task.Run<IEnumerable<IFilePath>>(() =>
        {
            var list = new List<IFilePath>();
            var entries = Directory
                .EnumerateFileSystemEntries(path)
                .GetEnumerator();

            while (entries.MoveNext() && !cancelToken.IsCancellationRequested)
            {
                var entry = entries.Current;
                var att = File.GetAttributes(entry);
                if ((att & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var native = new DirectoryInfo(entry);
                    list.Add(new FileSystemDirectory(native));
                }
                else
                {
                    var native = new FileInfo(entry);
                    list.Add(new FileSystemFile(native));
                }
            }
            return list;
        });


        public Task<IFile> GetFile(string path, CancellationToken cancellationToken = default)
        {
            var native = new FileInfo(path);
            IFile file = new FileSystemFile(native);
            return Task.FromResult(file);
        }


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
