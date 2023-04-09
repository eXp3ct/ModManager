using Core.Model;
using Core.Model.Data;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Sharing
{
    public class Artifacts
    {
        private readonly string _url = "https://api.bitbucket.org/2.0/repositories/mmmodmanager/sharing/downloads";
        private async Task<string> GetArtifactsAsync(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Share.token}");
            using var response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<Artifact>> GetListArtifactsAsync(string url)
        {
            var artifactsString = await GetArtifactsAsync(url);
            var artifactsData = JsonConvert.DeserializeObject<ArtifactsData>(artifactsString);

            return artifactsData.Values;
        }

        public async Task<byte[]> GetFileBytes(string fileName)
        {
            var url = $"{_url}/{fileName}";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {Share.token}");
            using var respose = await client.GetAsync(url);

            return await respose.Content.ReadAsByteArrayAsync();
        }
    }
}
