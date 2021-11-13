using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiny.Storage.Impl
{
    public class FileSystemFile : IFile
    {
        public FileSystemFile(FileInfo file) { }
        public string Name => throw new NotImplementedException();

        public bool Exists => throw new NotImplementedException();

        public DateTime? LastAccessed => throw new NotImplementedException();

        public DateTime? DateCreated => throw new NotImplementedException();

        public Task<Stream> OpenReadStream()
        {
            throw new NotImplementedException();
        }
    }
}
