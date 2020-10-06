using System.Collections.Generic;

namespace Margarina.LevelLoader
{
    public class LevelFile
    {

        public int Height { get; set; }
        public bool Infinite { get; set; }
        public List<LevelLayer> Layers { get; set; }
    }

    public class LevelLayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public List<Chunk> Chunks { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Startx { get; set; }
        public int Starty { get; set; }
    }

    public class Chunk
    {
        public List<int> Data { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
