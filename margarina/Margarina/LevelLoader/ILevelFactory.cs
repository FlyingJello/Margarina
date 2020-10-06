using Margarina.Models.World;

namespace Margarina.LevelLoader
{
    public interface ILevelFactory
    {
        Level GetLevel(string id);
    }
}