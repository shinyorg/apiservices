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


        public Task<IFileInfo> CreateDirectory(string path, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<IFileInfo>> GetDirectoryContents(string path, CancellationToken cancelToken = default)
        {
            await this.client.AutoConnectAsync(cancelToken).ConfigureAwait(false);
            await this.client.SetWorkingDirectoryAsync(path, cancelToken).ConfigureAwait(false);
            var listing = await this.client.GetListingAsync(cancelToken).ConfigureAwait(false);
            return listing.Select(x => new FtpFileInfo(x));
        }


        public Task<IFileInfo> GetFileInfo(string path, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}