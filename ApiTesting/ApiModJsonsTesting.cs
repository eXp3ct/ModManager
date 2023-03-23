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
    public class ApiModJsonsTesting
    {
        private readonly CurseModApiService _serivce = new();
        private readonly string JsonFolder = @"D:\Projects\C# Projects\ModManager\Jsons";

        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetMod_Json(List<int> modsIds)
        {
            foreach(var modId in modsIds)
            {
                var response = await _serivce.GetMod(modId);
                var expectedJson = await File.ReadAllTextAsync(@$"{JsonFolder}\{modId}.json");

                Assert.Equal(expectedJson, response);
            }
        }

        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetMods_Json(List<int> modIds)
        {
            var response = await _serivce.GetMods(modIds);
            var expectedJson = await File.ReadAllTextAsync($@"{JsonFolder}\getmodstest.json");

            Assert.Equal(expectedJson, response);
        }

        [Fact]
        public async Task GetCategories_Json()
        {
            var response = await _serivce.GetCategories(432, 6);
            var expcetedJson = await File.ReadAllTextAsync($@"{JsonFolder}\getcategoriestest.json");

            Assert.Equal(expcetedJson, response);
        }

        [Theory]
        [MemberData(nameof(TestIds.ModIdList), MemberType = typeof(TestIds))]
        public async Task GetModDescription_Json(List<int> modIds)
        {
            foreach(var modId in modIds)
            {
                var response = await _serivce.GetModDescription(modId);
                var expcetedJson = await File.ReadAllTextAsync($@"{JsonFolder}\{modId}-desc.json");

                Assert.Equal(expcetedJson, response);
            }
        }

        [Fact]
        public async Task SearchMod_Json()
        {
            var response = await _serivce.SearchMods(432, classId: 6, pageSize: 50, 
                modLoaderType: CurseForgeApiLib.Enums.ModLoaderType.Any,
                sortField: 0);
            var expectedJson = await File.ReadAllTextAsync($@"{JsonFolder}\searchmodstest.json");

            Assert.Equal(expectedJson, response);
        }

        [Fact]
        public async Task GetMinecraftVersion_Json()
        {
            var response = await _serivce.GetMinecraftVersions();
            var expcetedJson = await File.ReadAllTextAsync($@"{JsonFolder}\minecraftversionstest.json");

            Assert.Equal(expcetedJson, response);
        }

        [Fact]
        public async Task GetMinectaftModLoaders_Json()
        {
            var response = await _serivce.GetMinecraftModLoaders(includeAll: true);
            var expcetedJson = await File.ReadAllTextAsync($@"{JsonFolder}\minecraftmodloaderstest.json");

            Assert.Equal(expcetedJson, response);
        }
    }
}
