
using Core.Model;
using Core.Model.Data;
using Newtonsoft.Json;

namespace CurseForgeApiLib.HttpClients
{
    public class CurseApiClient : HttpClient 
    {
        private static readonly string ApiKey;
        public CurseApiClient()
        {
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }

        static CurseApiClient()
        {
            var text = File.ReadAllText(@"config\appkeys.json");
            var keys = JsonConvert.DeserializeObject<ApiKeysData>(text);

            ApiKey = keys.Tokens.First().Key;
        }
    }
}
