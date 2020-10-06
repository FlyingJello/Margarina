using System.Linq;
using Margarina.Models.Actors.Actions;
using Margarina.Models.World;
using Margarina.Utils;

namespace Margarina.Services
{
    public class ActorService : IActorService
    {
        private readonly WorldState _state;

        public ActorService(WorldState state)
        {
            _state = state;
        }

        public void UpdateActors(long tick)
        {
            var actors = _state.Actors;

            foreach (var actor in actors)
            {
                actor.Update(tick);
            }
        }

        public void MoveTo(string username, Point destination)
        {
            var actor = _state.Actors.Single(act => act.Id == username);
            var level = _state.Maps.Single(map => map.Id == actor.MapId).Level;
            var path = Astar.FindPath(level, actor.Position, destination);

            if (actor.CurrentAction != null && actor.CurrentAction is Movement movement)
            {
                movement.UpdateMovement(path, actor.Position);
            }
            else if (path.Count != 0)
            {
                actor.CurrentAction = new Movement(path, actor.Position);
            }
        }
    }
}
