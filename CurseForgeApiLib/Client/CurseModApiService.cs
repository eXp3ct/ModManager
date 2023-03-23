using CurseForgeApiLib.Behaivour;
using CurseForgeApiLib.Enums;
using CurseForgeApiLib.HttpClients;
using CurseForgeApiLib.Uris;
using Newtonsoft.Json;
using System.Text;

namespace CurseForgeApiLib.Client
{
    public class CurseModApiService : IGetMod, IGetMods, ISearchMods,
        IGetModDescription
    {
        private CurseApiClient Client { get; } = new();


        public async Task<string> GetMod(int modId)
        {
            using var response = await Client.GetAsync(CurseForgeUris.GetEndpoint(RequestType.GetMod, modId));

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetModDescription(int modId)
        {
            using var response = await Client.GetAsync(CurseForgeUris.GetEndpoint(RequestType.GetModDescription, modId));

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMods(List<int> modIds)
        {
            var url = CurseForgeUris.GetEndpoint(RequestType.GetMods);
            var requestBody = JsonConvert.SerializeObject(new { modIds });

            // Create a new HTTP request message with the POST method and request body
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            // Send the request and wait for the response
            using var response = await Client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SearchMods(int gameId, int classId = 0,
            int categoryId = 0, string gameVersion = "", string searchFilter = "",
            SearchSortFields sortField = 0, string sortOrder = "asc",
            ModLoaderType modLoaderType = ModLoaderType.Any, int gameVersionTypeId = 0,
            int authorId = 0, string slug = "", int index = 0, int pageSize = 0)
        {
            var queryString = new StringBuilder();
            queryString.Append($"gameId={gameId}");
            if (classId != 0) queryString.Append($"&classId={classId}");
            if (categoryId != 0) queryString.Append($"&categoryId={categoryId}");
            if (!string.IsNullOrEmpty(gameVersion)) queryString.Append($"&gameVersion={Uri.EscapeDataString(gameVersion)}");
            if (!string.IsNullOrEmpty(searchFilter)) queryString.Append($"&searchFilter={Uri.EscapeDataString(searchFilter)}");
            queryString.Append($"&sortField={(int)sortField}");
            queryString.Append($"&sortOrder={sortOrder}");
            queryString.Append($"&modLoaderType={(int)modLoaderType}");
            if (gameVersionTypeId != 0) queryString.Append($"&gameVersionTypeId={gameVersionTypeId}");
            if (authorId != 0) queryString.Append($"&authorId={authorId}");
            if (!string.IsNullOrEmpty(slug)) queryString.Append($"&slug={Uri.EscapeDataString(slug)}");
            queryString.Append($"&index={index}");
            queryString.Append($"&pageSize={pageSize}");

            var url = $"{CurseForgeUris.GetEndpoint(RequestType.SearchMod)}?{queryString}";

            using var httpClient = new HttpClients.CurseApiClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
