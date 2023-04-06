using Octokit;
using System.IO.Compression;
using System.Net.Http.Headers;
namespace Updater
{
    public class GitUpdater
    {
        private readonly string _owner = "eXp3ct";
        private readonly string _repo = "ModManager";
        private readonly string _token = "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";
        private string _version;
        private GitHubClient _client;
        private Release _latestRelease;
        public GitUpdater() 
        {
            /*_version = version;
            _client = new GitHubClient(new Octokit.ProductHeaderValue("ModManager"))
            {
                Credentials = new Credentials(_token)
            };*/

            
        }


        public async Task DownloadRepo()
        {
            
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "ATCTT3xFfGN0kTr3-yMRz5WMXu6JcpIeYCXz6qhiDuwUYovLvHdJWjp8IbTKqz6yYbrwHp4Pn4QE3PUErjTo3VmvD0yVmx15H4NMDQG9KsUp7VLytQWkAmPcs8FkTMxfCcmfzkTQG198NTdxVs7HCU4y7jrcZy7FzaVjNeUfn4b-aPfys4abgBo=A7E028FC");

            using var response = await client.GetAsync("https://api.bitbucket.org/2.0/repositories/mmmodmanager/updates/downloads/release.zip");
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(response.StatusCode.ToString());
            else
            {
                var stream = await response.Content.ReadAsByteArrayAsync();
                using var fileStream = new FileStream(@"D:\Projects\Git\gitest\release.zip", System.IO.FileMode.Create);
                fileStream.Write(stream, 0, stream.Length);

                
            }
        }


        public static async Task ExtractZipArchiveAsync(string archivePath, string extractPath)
        {
            // Ensure that the extract directory exists
            Directory.CreateDirectory(extractPath);

            using var archive = ZipFile.OpenRead(archivePath);
            foreach (var entry in archive.Entries)
            {
                var filePath = Path.Combine(extractPath, entry.FullName);

                // Create directory for the file if it doesn't exist
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Extract the entry to file
                if (entry.Length > 0)
                {
                    using var entryStream = entry.Open();
                    using var fileStream = new FileStream(filePath, System.IO.FileMode.Create, FileAccess.Write);
                    await entryStream.CopyToAsync(fileStream);
                }
            }
        }
    }
}