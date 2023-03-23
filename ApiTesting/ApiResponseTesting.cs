using CurseForgeApiLib.Client;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.Uris;
using CurseForgeApiLib.HttpClients;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;
using ApiTesting.TestData;
using System;

namespace ApiTesting
{
    public class ApiResponseTesting
    {
        private readonly CurseApiClient _client = new();

        [Theory]
        [MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
        public async Task GetMod_FileStatusCode(int modId, List<int> fileIds)
        {
            foreach(var fileId in fileIds)
            {
                var url = CurseForgeUris.GetEndpoint(RequestType.GetModFile, modId: modId, fileId: fileId);

                using var response = await _client.GetAsync(url);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetModFiles_StatusCode(List<int> modIds)
        {
            foreach(var modId in modIds)
            {
                var url = CurseForgeUris.GetEndpoint(RequestType.GetModFiles, modId: modId);

                using var response = await _client.GetAsync(url);

                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }            
        }
        [Theory]
        [MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
        public async Task GetModFileDownloadUrl_StatusCode(int modId, List<int> fileIds)
        {
            foreach (var fileId in fileIds)
            {
                var url = CurseForgeUris.GetEndpoint(RequestType.GetModFileDownloadUrl, modId: modId, fileId: fileId);

                using var response = await _client.GetAsync(url);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetMods_StatusCode(List<int> modIds)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMods);

            var requestBody = JsonConvert.SerializeObject(new { modIds });

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetMod_StatusCode(List<int> modIds)
        {
            foreach(var modId in modIds)
            {
                var url = CurseForgeUris.GetEndpoint(RequestType.GetMod, modId);

                var response = await _client.GetAsync(url);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            } 
        }

        [Theory]
        [InlineData(432, 6)]
        public async Task GetCategories_StatusCode(int gameId, int classId)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetCategories) + $"?gameId={gameId}&classId={classId}";

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(TestIds.FileIdList), MemberType = typeof(TestIds))]
        public async Task GetFiles_StatusCode(List<int> fileIds)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetFiles);
            var requestBody = JsonConvert.SerializeObject(new { fileIds });

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(null, true)]
        public async Task GetMinecraftModLoaders_StatusCode(string versions, bool includeALl)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMinecraftModLoaders)
                + $"?versions={versions}&includeAll={includeALl}";

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(false)]
        public async Task GetMinecraftVersions_StatusCode(bool sortDescending)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMinecraftVersions) + $"?sortDescending={sortDescending}";

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetModDescription_StatusCode(List<int> modIds)
        {
            foreach(var modId in modIds)
            {
                using var response = await _client.GetAsync(CurseForgeUris.GetEndpoint(RequestType.GetModDescription, modId));

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task SearchMods_StatusCode()
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.SearchMod);

            using var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}