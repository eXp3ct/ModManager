using CurseForgeApiLib.Client;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.Uris;
using CurseForgeApiLib.WebClient;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace ApiTesting
{
    public class ApiResponseTesting
    {
        [Fact]
        public async Task GetModFileStatusCode()
        {
            using var client = new Client();
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFile, modId:250277, fileId: 4417968);

            using var response = await client.GetAsync(url);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetModFilesStatusCode()
        {
            using var client = new Client();
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFiles, 222880);

            using var response = await client.GetAsync(url);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetModFileDownloadUrlStatusCode()
        {
            using var client = new Client();
            var url = CurseForgeUris.GetEndpoint(RequestType.GetModFileDownloadUrl, modId: 250277, fileId: 4417968);

            using var response = await client.GetAsync(url);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetModsStatusCode()
        {
            using var client = new Client();
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMods);

            // Create a list of mod IDs to send in the request body
            var modIds = new List<int> { 222880, 325739, 291737, 390003 };

            // Serialize the list of mod IDs to JSON
            var requestBody = JsonConvert.SerializeObject(new { modIds });

            // Create a new HTTP request message with the POST method and request body
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            // Send the request and wait for the response
            using var response = await client.SendAsync(request);

            // Assert that the status code is OK
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}