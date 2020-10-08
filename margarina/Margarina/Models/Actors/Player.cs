using System.Collections.Generic;

namespace Margarina.Models.Actors
{
    public class Player : Actor
    {
        public string Name { get; }
        public int FieldOfView { get; set; }
        public List<string> ChatHistory { get; set; }

        public Player(string name) : base(name)
        {
            Type = "player";
            ChatHistory = new List<string>();
            Name = name;
            Speed = 120;
            FieldOfView = 100;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public void Login()
        {
            ChatHistory.Clear();
        }
    }
}
