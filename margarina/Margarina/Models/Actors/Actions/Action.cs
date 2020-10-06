namespace Margarina.Models.Actors.Actions
{
    public abstract class Action
    {
        public ActionType Type { get; set; }

        public abstract bool UpdateAction(long tick, Actor actor);
    }

    public enum ActionType
    {
        Movement = 1,
        Routine = 2,
        Wait = 0
    }
}
