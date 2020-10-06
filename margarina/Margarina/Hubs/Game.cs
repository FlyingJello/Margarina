using System;
using System.Threading.Tasks;
using Margarina.Services;
using Margarina.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Margarina.Hubs
{
    [Authorize]
    public class Game : Hub
    {
        private readonly IActorService _actorService;
        private readonly IPlayerService _playerService;
        private readonly IChatService _chatService;

        public Game(IActorService actorService, IPlayerService playerService, IChatService chatService)
        {
            _actorService = actorService;
            _playerService = playerService;
            _chatService = chatService;
        }

        public override Task OnConnectedAsync()
        {
            _playerService.Login(this.GetUsername());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _playerService.Disconnect(this.GetUsername());
            return base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("move_request")]
        public void MoveRequest(Point destination)
        {
            _actorService.MoveTo(this.GetUsername(), destination);
        }

        [HubMethodName("chat")]
        public void Chat(string text)
        {
            _chatService.ChatLocally(this.GetUsername(), text);
        }
    }
}
