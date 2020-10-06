using Margarina.Utils;

namespace Margarina.Models.World
{
    public class Layer
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Tile[][] Tiles { get; set; }

        public Tile GetTileAt(Point point)
        {
            return GetTileAt(point.X, point.Y);
        }

        public Tile GetTileAt(int x, int y)
        {
            return IsInBounds(x, y) ? Tiles[x][y] : default;
        }

        public bool IsInBounds(int x, int y)
        {
            return (x >= 0) && (x < Width) && (y >= 0) && (y < Height);
        }
    }
}
