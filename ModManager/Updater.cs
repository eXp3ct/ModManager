using AutoUpdaterDotNET;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModManager
{
    public class Updater : IAuthentication
    {
        private readonly string token = "ATCTT3xFfGN03zmTmXROwLADUKT9rnZTYjYg1R5-uxmkwS9tfvpRADpQWlWwKxDidR6osLZ3lh_UM0N2jHN3gOCSCRG03VsqMg61Siw6SZNMeKbvhagoOZgH2zvvIVhXDRUrXo3_7LoHT8wccj74r7j2f39kqSDyiIqBzvZH7oYMhaMH4QxRq10=E184FE8E";

        public Updater()
        {
            AutoUpdater.InstallationPath = Assembly.GetExecutingAssembly().Location;
        }

        public void CheckForUpdates()
        {
            if(EnsureAuth())
                AutoUpdater.Start("https://api.bitbucket.org/2.0/repositories/mmmodmanager/updates/downloads/updates.xml");
        }
        public bool EnsureAuth()
        {
            if(AutoUpdater.BasicAuthChangeLog == null)
                AutoUpdater.BasicAuthChangeLog = this;
            if(AutoUpdater.BasicAuthDownload == null)
                AutoUpdater.BasicAuthDownload = this;
            if(AutoUpdater.BasicAuthXML == null)
                AutoUpdater.BasicAuthXML = this;

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
