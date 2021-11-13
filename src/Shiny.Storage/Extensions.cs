namespace Shiny.Storage
{
    public static class Extensions
    {
        public static bool IsFile(this IFilePath path) => path is IFile;
        public static bool IsDirectory(this IFilePath path) => path is IDirectory;
    }
}
