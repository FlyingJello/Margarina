namespace Margarina.Models.World
{
    public class Tile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; }
        public int Y { get; }

        public Tile(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public Tile(Tile tile)
        {
            Id = tile.Id;
            Name = tile.Name;
            X = tile.X;
            Y = tile.Y;
        }
    }
}
