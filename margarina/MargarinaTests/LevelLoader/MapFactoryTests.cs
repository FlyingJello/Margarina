using System.IO;
using System.Text.Json;
using Margarina.LevelLoader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MargarinaTests.LevelLoader
{
    [TestClass]
    public class MapFactoryTests
    {
        [TestMethod]
        public void FromLevelFile_Should_ConvertMap()
        {
            var content = File.ReadAllText("test_level.json");
            var file = JsonSerializer.Deserialize<LevelFile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var map = new LevelFactory().ConvertFileToLevel(file);

            Assert.IsNotNull(map);
        }
    }
}