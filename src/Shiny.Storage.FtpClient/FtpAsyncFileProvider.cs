using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shiny.Storage.FtpClient
{
    public class FtpAsyncFileProvider : IAsyncFileProvider
    {
        readonly FluentFTP.FtpClient client;


        public FtpAsyncFileProvider()
        {
            this.client = new FluentFTP.FtpClient("123.123.123.123", "david", "pass123");

        }
        public Task<IDirectory> CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IFilePath>> GetDirectoryContents(string path, CancellationToken cancelToken = default)
        {
            await this.client.AutoConnectAsync(cancelToken);

            await this.client.SetWorkingDirectoryAsync(path, cancelToken);
            var listing = await this.client.GetListingAsync(cancelToken);
            foreach (var item in listing)
            {
                // TODO: ignore links?
                if (item.Type == FluentFTP.FtpFileSystemObjectType.Directory)
                {

                }
            }
            return Enumerable.Empty<IFilePath>();
        }


        public Task<IFile> GetFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}