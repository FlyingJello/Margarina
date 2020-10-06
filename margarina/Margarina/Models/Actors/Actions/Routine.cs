using System.Collections.Generic;

namespace Margarina.Models.Actors.Actions
{
    public class Routine : Action
    {
        public Queue<Action> Cycle { get; set; }
        public Action CurrentStep { get; set; }
        public bool IsInfinite { get; set; }

        public Routine(List<Action> steps, bool isInfinite)
        {
            Type = ActionType.Routine;
            Cycle = new Queue<Action>(steps);
            IsInfinite = isInfinite;
            CurrentStep = Cycle.Peek();
        }

        public override bool UpdateAction(long tick, Actor actor)
        {
            var isFinished = CurrentStep.UpdateAction(tick, actor);

            if (isFinished && IsInfinite)
            {
                CurrentStep = Cycle.Dequeue();
                Cycle.Enqueue(CurrentStep);
                return false;
            }

            return isFinished;
        }
    }
}
