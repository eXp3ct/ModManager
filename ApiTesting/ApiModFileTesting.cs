using ApiTesting.TestData;
using CurseForgeApiLib.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiTesting
{
    public class ApiModFileTesting
    {
        private readonly CurseModFileApiService _service = new();
        private readonly string JsonFolder = @"D:\Projects\C# Projects\ModManager\Jsons";

        [Theory]
        [MemberData(nameof(TestIds.FileIdList), MemberType = typeof(TestIds))]
        public async Task GetFiles_Json(List<int> fileIds)
        {
            var response = await _service.GetFiles(fileIds);
            var expectedJson = await File.ReadAllTextAsync($@"{JsonFolder}\getfilestest.json");

            Assert.Equal(expectedJson, response);
        }

        [Theory]
        [MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
        public async Task GetModFile_Json(int modId, List<int> fileIds)
        {
            foreach(var fileId in fileIds)
            {
                var response = await _service.GetModFile(modId, fileId);
                var expectedJson = await File.ReadAllTextAsync($@"{JsonFolder}\{fileId}.json");

                Assert.Equal(expectedJson, response);
            }
        }

        [Theory]
        [MemberData(nameof(TestIds.ModFileIdList), MemberType = typeof(TestIds))]
        public async Task GetModFileDownloadUrl_Json(int modId, List<int> fileIds)
        {
            foreach(var fileId in  fileIds)
            {
                var response = await _service.GetModFileDownloadUrl(modId, fileId);
                var expectedJson = await File.ReadAllTextAsync($@"{JsonFolder}\{fileId}-url.json");

                Assert.Equal(expectedJson, response);
            }
        }

        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetModFiles_Json(List<int> modIds)
        {
            foreach (var modId in modIds)
            {
                var response = await _service.GetModFiles(modId, pageSize: 50);
                var expectedJson = await File.ReadAllTextAsync($@"{JsonFolder}\{modId}-files.json");

                Assert.Equal(expectedJson, response);
            }
        }
    }
}
