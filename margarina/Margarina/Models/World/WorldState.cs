using System.Collections.Generic;
using System.Linq;
using Margarina.Models.Actors;
using Margarina.Utils;

namespace Margarina.Models.World
{
    public class WorldState
    {
        public List<Map> Maps { get; set; }
        public List<Actor> Actors { get; set; }
        public HashSet<Player> ConnectedPlayers { get; set; }

        public WorldState()
        {
            Maps = new List<Map>();
            Actors = new List<Actor>();
            ConnectedPlayers = new HashSet<Player>();
        }

        public WorldState GetStateScope(Player player)
        {
            var actorsInRange = WorldUtils.GetActorsInRange(this, player, player.FieldOfView);

            var disconnectedPlayers = Actors.Where(actor => actor is Player ply && !ConnectedPlayers.Contains(ply));

            return new WorldState
            {
                Maps = Maps,
                Actors = actorsInRange.Except(disconnectedPlayers).ToList()
            };
        }
    }
}
