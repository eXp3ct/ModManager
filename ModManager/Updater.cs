using AutoUpdaterDotNET;
using Core.Model;
using Core.Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModManager
{
    public class Updater : IAuthentication
    {
        private static readonly string token;

        public Updater()
        {
            AutoUpdater.InstallationPath = Assembly.GetExecutingAssembly().Location;
            AutoUpdater.LetUserSelectRemindLater = false;
        }

        static Updater()
        {
            var text = File.ReadAllText(@"config\appkeys.json");
            var keys = JsonConvert.DeserializeObject<ApiKeysData>(text);

            token = keys.Tokens.First(api => api.Name == "Bitbucket").Key;
        }

        public void CheckForUpdates()
        {
            if(EnsureAuth())
                AutoUpdater.Start("https://api.bitbucket.org/2.0/repositories/mmmodmanager/updates/downloads/updates.xml");
        }
        public bool EnsureAuth()
        {
            AutoUpdater.BasicAuthChangeLog ??= this;
            AutoUpdater.BasicAuthDownload ??= this;
            AutoUpdater.BasicAuthXML ??= this;

            return true;
        }
        public void Apply(ref MyWebClient webClient)
        {
            WebHeaderCollection webHeaderCollection = new()
            {
                { "Authorization", $"Bearer {token}" }
            };
            webClient.Headers.Add(webHeaderCollection);
        }
    }
}
