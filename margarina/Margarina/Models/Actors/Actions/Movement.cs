using System;
using System.Collections.Generic;
using Margarina.Utils;

namespace Margarina.Models.Actors.Actions
{
    public class Movement : Action
    {
        public PointF Position { get; set; }
        public Point NextStep { get; set; }
        public Stack<Point> Path { get; set; }

        public Movement(IEnumerable<Point> path, Point origin)
        {
            Type = ActionType.Movement;
            Path = new Stack<Point>(path);
            Position = new PointF(origin.X, origin.Y);  
            NextStep = Path.Pop();
        }

        public override bool UpdateAction(long tick, Actor actor)
        {
            const double ups = 1000.0 / Constants.TickLength;

            var speed = actor.Speed * Constants.GlobalSpeedFactor;
            var deltaX = NextStep.X - Position.X;
            var deltaY = NextStep.Y - Position.Y;
            var distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

            var step = speed / ups;
            var newDistance = distance - step;

            if (newDistance <= 0)
            {
                actor.Position = NextStep;

                if (Path.Count == 0)
                {
                    return true;
                }

                Position = new PointF(NextStep);
                NextStep = Path.Pop();

                var extra = Math.Abs(newDistance);
                deltaX = NextStep.X - Position.X;
                deltaY = NextStep.Y - Position.Y;
                distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
                newDistance = distance - extra;
            }

            var ratio = 1 - (newDistance / distance);

            Position = new PointF(Position.X + deltaX * ratio, Position.Y + deltaY * ratio);

            return false;
        }

        public void UpdateMovement(List<Point> path, Point position)
        {
            if (path.Count == 0)
            {
                NextStep = position;
            }
            else
            {
                Path = new Stack<Point>(path);
                NextStep = Path.Pop();
            }
        }
    }
}
