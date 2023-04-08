using Core.Model.Data;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Sharing
{
    public class Share
    {
        private static readonly string token;
        private readonly string _url = "https://api.bitbucket.org/2.0/repositories/mmmodmanager/sharing/downloads";
        static Share()
        {
            var text = File.ReadAllText(@"config\appkeys.json");
            var keys = JsonConvert.DeserializeObject<ApiKeysData>(text);

            token = keys.Tokens.First(api => api.Name == "BitbucketSharing").Key;
        }
        public async Task<bool> UploadFiles(string path)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");

            using var content = new MultipartFormDataContent();
            var fileBytes = await File.ReadAllBytesAsync(path);
            var fileContentBytes = new ByteArrayContent(fileBytes);
            fileContentBytes.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            content.Add(fileContentBytes, "files", Path.GetFileName(path));

            using var response = await client.PostAsync(_url, content);

            return response.IsSuccessStatusCode;
        }
    }
}