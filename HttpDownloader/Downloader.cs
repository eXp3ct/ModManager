using CurseForgeApiLib.HttpClients;
using HttpDownloader.Behaivour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpDownloader
{
    public class Downloader : IDownloader
    {
        public async Task<byte[]> Download(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Array.Empty<byte>();
            using var client = new CurseApiClient();
            using var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
