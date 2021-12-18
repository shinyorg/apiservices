using System.IO;
using System.Threading.Tasks;


namespace Shiny.Storage
{
    public static class Extensions
    {
        public static bool IsFile(this IFilePath path) => path is IFile;
        public static bool IsDirectory(this IFilePath path) => path is IDirectory;


        public static async Task<string> ReadFileAsString(this IFile file)
        {
            using (var stream = await file.OpenStream(false).ConfigureAwait(false))
                using (var reader = new StreamReader(stream))
                    return await reader.ReadToEndAsync();
        }
    }
}
