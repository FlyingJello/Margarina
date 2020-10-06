using System.Collections.Generic;

namespace Margarina.Models.World
{
    public class Level
    {
        public string Name { get; set; }
        public List<Layer> Layers { get; set; }
    }
}
