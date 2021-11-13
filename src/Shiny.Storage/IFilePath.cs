namespace Shiny.Storage
{
    public interface IFilePath
    {
        string Name { get; }
        string FullName { get; }
        bool Exists { get; }

        DateTimeOffset? LastAccessTime { get; }
        DateTimeOffset? LastWriteTime { get; }
        DateTimeOffset? CreationTime { get; }
    }
}
