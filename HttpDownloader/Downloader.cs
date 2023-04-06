using CurseForgeApiLib.HttpClients;
using HttpDownloader.Behaivour;
using Logging;

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

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                LoggerService.Logger.Error($"Cannot fetch file from url {url}");
                return Array.Empty<byte>();
            }
        }
    }
}
