using System.IO;


namespace Shiny.Storage.Impl
{
    public static class Utils
    {
        public static bool IsDirectory(string path)
        {
            var att = File.GetAttributes(path);
            return ((att & FileAttributes.Directory) == FileAttributes.Directory);
            //{
            //    var native = new DirectoryInfo(entry);
            //    list.Add(new FileSystemDirectory(native));
            //}
            //else
            //{
            //    var native = new FileInfo(entry);
            //    list.Add(new FileSystemFile(native));
            //}
        }
    }
}
