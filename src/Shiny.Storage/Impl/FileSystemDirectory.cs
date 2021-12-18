using System;
using System.IO;

namespace Shiny.Storage.Impl
{
    public class FileSystemDirectory : IDirectory
    {
        readonly DirectoryInfo directory;
        public FileSystemDirectory(DirectoryInfo directory) => this.directory = directory;

        public string Name => this.directory.Name;
        public string FullName => this.directory.FullName;
        public bool Exists => this.directory.Exists;

        public DateTimeOffset? LastAccessTime => this.directory.LastAccessTimeUtc;
        public DateTimeOffset? LastWriteTime => this.directory.LastWriteTimeUtc;
        public DateTimeOffset? CreationTime => this.directory.CreationTimeUtc;
    }
}
