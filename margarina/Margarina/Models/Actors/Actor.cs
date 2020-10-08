using Margarina.Models.Actors.Actions;
using Margarina.Utils;

namespace Margarina.Models.Actors
{
    public abstract class Actor
    {
        public string Id { get; }
        public string Type { get; set; }
        public string SpriteId { get; set; }
        public Point Position { get; set; }
        public string MapId { get; set; }
        public int Speed { get; set; }
        public Action CurrentAction { get; set; }

        protected Actor(string id)
        {
            Id = id;
        }

        public virtual void Update(long tick)
        {
            if (CurrentAction == null)
            {
                return;
            }

            var isActionFinished = CurrentAction.UpdateAction(tick, this);

            if (isActionFinished)
            {
                CurrentAction = default;
            }
        }
    }
}
