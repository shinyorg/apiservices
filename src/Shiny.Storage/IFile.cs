﻿namespace Shiny.Storage
{
    public interface IFile : IFilePath
    {
        long Size { get; }
        Task<Stream> OpenStream();
    }
}