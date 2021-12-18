using System.IO;
using System.Threading.Tasks;

namespace Shiny.Storage
{
    public interface IFile : IFilePath
    {
        long Size { get; }
        Task<Stream> OpenStream(bool forWrite);
    }
}
