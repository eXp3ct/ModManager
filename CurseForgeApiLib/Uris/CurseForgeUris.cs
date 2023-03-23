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
        private static readonly Dictionary<RequestType, string> Endpoints = new()
        {
            {RequestType.SearchMod, $"{BaseUrl}/v1/mods/search" },
            {RequestType.GetMod, $"{BaseUrl}/v1/mods/{{modId}}" },
            {RequestType.GetMods, $"{BaseUrl}/v1/mods" },
            {RequestType.GetModDescription, $"{BaseUrl}/v1/mods/{{modId}}/description" },
            {RequestType.GetCategories, $"{BaseUrl}/v1/categories" },
            {RequestType.GetMinecraftVersions, $"{BaseUrl}/v1/minecraft/version" },
            {RequestType.GetMinecraftModLoaders, $"{BaseUrl}/v1/minecraft/modloader" },
            {RequestType.GetModFile, $"{BaseUrl}/v1/mods/{{modId}}/files/{{fileId}}" },
            {RequestType.GetModFiles, $"{BaseUrl}/v1/mods/{{modId}}/files" },
            {RequestType.GetFiles, $"{BaseUrl}/v1/mods/files" },
            {RequestType.GetModFileDownloadUrl, $"{BaseUrl}/v1/mods/{{modId}}/files/{{fileId}}/download-url" },
        };
        public static string GetEndpoint(RequestType requestType, int? modId = null, int? fileId = null)
        {
            if (Endpoints.TryGetValue(requestType, out var endpoint))
            {
                var modifiedEndpoint = endpoint;

                if (modId != null && modifiedEndpoint.Contains("{modId}"))
                {
                    modifiedEndpoint = modifiedEndpoint.Replace("{modId}", modId.ToString());
                }

                if (fileId != null && modifiedEndpoint.Contains("{fileId}"))
                {
                    modifiedEndpoint = modifiedEndpoint.Replace("{fileId}", fileId.ToString());
                }

                return modifiedEndpoint;
            }
            else
            {
                throw new ArgumentException($"Invalid request type: {requestType}");
            }
        }
    }
}
