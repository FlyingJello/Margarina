using System.Collections.Generic;
using System.Linq;
using Margarina.Models.Actors;
using Margarina.Models.World;
using Margarina.Utils;

namespace Margarina.Services
{
    public class ChatService : IChatService
    {
        private readonly WorldState _state;

        public ChatService(WorldState state)
        {
            _state = state;
        }

        public void ChatLocally(string username, string text)
        {
            var player = (Player)_state.Actors.Single(act => act.Id == username);
            var playersInRange = WorldUtils.GetActorsInRange(_state, player, player.FieldOfView).OfType<Player>();

            var formattedMessage = $"[{username}]: {text}";

            foreach (var otherPlayers in playersInRange)
            {
                otherPlayers.ChatHistory.Add(formattedMessage);
            }
        }
    }
}
