namespace Margarina.Models.Actors.Actions
{
    public class Wait : Action
    {
        public int Delay { get; set; }
        private int _remainder;

        public Wait(int delay)
        {
            Type = ActionType.Wait;
            Delay = delay;
            _remainder = delay;
        }

        public override bool UpdateAction(long tick, Actor actor)
        {
            _remainder -= Constants.TickLength;

            if (_remainder <= 0)
            {
                return true;
            }

            return false;
        }
    }
}
