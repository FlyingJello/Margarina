using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Margarina.Hubs;
using Margarina.LevelLoader;
using Margarina.Models.Actors;
using Margarina.Models.World;
using Margarina.Services;
using Margarina.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Margarina
{
    public class MainLoop : BackgroundService
    {
        private readonly IHubContext<Game> _gameContext;
        private readonly ILogger<MainLoop> _logger;
        private readonly WorldState _state;

        private readonly ILevelFactory _levelFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MainLoop(ILogger<MainLoop> logger, IHubContext<Game> gameContext, WorldState state, ILevelFactory levelFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _gameContext = gameContext;
            _state = state;
            _levelFactory = levelFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var map = new Map { Id = "testmap", Level = _levelFactory.GetLevel("test_level") };
            var doug = new Player("Doug") { SpriteId = "player", Position = new Point(16, 12), MapId = map.Id, Speed = 120 };
            var eugenie = new Player("Eugenie") { SpriteId = "player", Position = new Point(16, 14), MapId = map.Id, Speed = 120 };

            _state.Actors.Add(doug);
            _state.Actors.Add(eugenie);
            _state.Maps.Add(map);

            var previousExecution = DateTimeOffset.Now;
            var tick = 0L;

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
                    actorService.UpdateActors(tick);
                }

                var delta = previousExecution + TimeSpan.FromMilliseconds(Constants.TickLength) - DateTimeOffset.Now;
                var delay = Math.Max((int)delta.TotalMilliseconds, 0);

                if (delay == 0)
                {
                    _logger.LogWarning($"Loop running late : {(int)delta.TotalMilliseconds} ms");
                }

                var clientTasks = new List<Player>(_state.ConnectedPlayers).Select(player =>
                    _gameContext.Clients.User(player.Name).SendAsync("tick", _state.GetStateScope(player), stoppingToken));

                await Task.WhenAll(clientTasks);

                await Task.Delay(delay, stoppingToken);
                previousExecution = DateTimeOffset.Now;
                tick++;
            }
        }
    }
}
