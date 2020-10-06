using System.Collections.Generic;
using System.Linq;
using Margarina.Models.Actors;
using Margarina.Models.World;

namespace Margarina.Utils
{
    public static class WorldUtils
    {
        public static IEnumerable<Actor> GetActorsInRange(WorldState state, Player player, int range)
        {
            return state.Actors.Where(actor => actor.Position.X <= player.Position.X + range &&
                                                      actor.Position.X >= player.Position.X - range &&
                                                      actor.Position.Y <= player.Position.Y + range &&
                                                      actor.Position.Y >= player.Position.Y - range);
        }
    }
}
