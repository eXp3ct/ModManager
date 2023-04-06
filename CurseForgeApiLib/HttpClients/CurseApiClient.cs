
namespace CurseForgeApiLib.HttpClients
{
    public class CurseApiClient : HttpClient 
    {
        private readonly string ApiKey = "$2a$10$ELJ0FcxFLTXGmPRWzynsYOj051DwZtEuuyK8ALgZUho3CIuYbvQZS";
        public CurseApiClient()
        {
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }
    }
}
