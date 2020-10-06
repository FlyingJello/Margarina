using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.Json;
using Margarina.Models.World;

namespace Margarina.LevelLoader
{
    public class LevelFactory : ILevelFactory
    {
        private readonly ConcurrentDictionary<string, Level> _maps;

        public LevelFactory()
        {
            _maps = new ConcurrentDictionary<string, Level>();
        }

        public Level GetLevel(string id)
        {
            if (_maps.TryGetValue(id, out var map))
            {
                return map;
            }

            var levelFile = LoadLevelFile(id);
            var newLevel = ConvertFileToLevel(levelFile);

            return _maps.GetOrAdd(id, newLevel);
        }

        private LevelFile LoadLevelFile(string name)
        {
            var content = File.ReadAllText($"{name}.json");
            return JsonSerializer.Deserialize<LevelFile>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public Level ConvertFileToLevel(LevelFile levelFile)
        {
            var map = new Level
            {
                Layers = levelFile.Layers.Select(layer => new Layer
                {
                    Tiles = ConvertLayer(layer),
                    Name = layer.Name,
                    Height = layer.Height,
                    Width = layer.Width,
                    IsVisible = layer.Visible
                }).ToList()
            };

            return map;
        }

        private static Tile[][] ConvertLayer(LevelLayer layer)
        {
            var x = 0;

            var output = new Tile[layer.Width][];
            for (var i = 0; i < layer.Width; i++)
            {
                output[i] = new Tile[layer.Height];
            }

            foreach (var chunk in layer.Chunks)
            {
                var offsetX = chunk.X - layer.Startx;
                var offsetY = chunk.Y - layer.Starty;

                var y = 0;

                foreach (var tile in chunk.Data)
                {
                    var newOffsetX = x + offsetX;
                    var newOffsetY = y + offsetY;

                    output[newOffsetX][newOffsetY] = new Tile(tile, newOffsetX, newOffsetY);

                    x++;

                    if (x == chunk.Width)
                    {
                        y++;
                        x = 0;
                    }
                }
            }

            return output;
        }
    }
}
