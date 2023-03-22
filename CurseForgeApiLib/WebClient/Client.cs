using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.WebClient
{
    public class Client : HttpClient
    {
        private readonly string ApiKey = "$2a$10$ELJ0FcxFLTXGmPRWzynsYOj051DwZtEuuyK8ALgZUho3CIuYbvQZS";
        public Client()
        {
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Add("x-api-key", ApiKey);
        }
    }
}
