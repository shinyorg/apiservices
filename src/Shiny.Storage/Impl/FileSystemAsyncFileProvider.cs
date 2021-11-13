namespace Shiny.Storage.Impl
{
    public class FileSystemAsyncFileProvider : IAsyncFileProvider
    {
        //public FileSystemAsyncFileProvider(string? rootDirectory = null)
        //{
        //    Directory.GetDirectoryRoot
        //}


        public Task<IDirectory> CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken) => Task.Run<IEnumerable<IFilePath>>(() =>
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


        public Task<IFile> GetFile(string path)
        {
            var native = new FileInfo(path);
            IFile file = new FileSystemFile(native);
            return Task.FromResult(file);
        }
    }
}
