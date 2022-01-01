using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Storage
{
    public static class Extensions
    {
        public static async Task<Stream> CreateFile(this IAsyncFileProvider provider, string path, CancellationToken cancellationToken = default)
        {
            var file = await provider.GetFileInfo(path, cancellationToken).ConfigureAwait(false);
            if (file.Exists)
            {
                // TODO?
            }
            return await file.OpenStream(true).ConfigureAwait(false);
        }

        public static async Task<string> ReadFileAsString(this IFileInfo file)
        {
            using (var stream = await file.OpenStream(false).ConfigureAwait(false))
                using (var reader = new StreamReader(stream))
                    return await reader.ReadToEndAsync();
        }
    }
}
