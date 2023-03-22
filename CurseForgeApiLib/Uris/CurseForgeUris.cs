using CurseForgeApiLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeApiLib.Uris
{
    public static class CurseForgeUris
    {
        private static readonly string BaseUrl = "https://api.curseforge.com";
        public static readonly Dictionary<RequestType, string> Endpoints = new()
        {
            {RequestType.SearchMod, $"{BaseUrl}/v1/mods/search" },
            {RequestType.GetMod, $"{BaseUrl}/v1/mods/{{modId}}" },
            {RequestType.GetMods, $"{BaseUrl}/v1/mods" },
            {RequestType.GetModDescription, $"{BaseUrl}/v1/mods/{{modId}}/description" },
            {RequestType.GetCategories, $"{BaseUrl}/v1/categories" }
        };
        public static string GetEndpoint(RequestType requestType, int? modId = null)
        {
            if (Endpoints.TryGetValue(requestType, out var endpoint))
            {
                if (modId != null && endpoint.Contains("{modId}"))
                {
                    return endpoint.Replace("{modId}", modId.ToString());
                }
                else
                {
                    return endpoint;
                }
            }
            else
            {
                throw new ArgumentException($"Invalid request type: {requestType}");
            }
        }
    }
}
