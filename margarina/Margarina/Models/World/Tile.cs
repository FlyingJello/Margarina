namespace Margarina.Models.World
{
    public class Tile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile(int id)
        {
            Id = id;
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
