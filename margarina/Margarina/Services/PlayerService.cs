using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Margarina.Configuration;
using Margarina.Models;
using Margarina.Models.Actors;
using Margarina.Models.World;
using Margarina.Persistence;
using Margarina.Utils;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Margarina.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly CloudTableClient _tableClient;
        private readonly AuthenticationConfig _authConfig;
        private readonly WorldState _state;

        public PlayerService(IStorageTableFactory factory, IOptions<AuthenticationConfig> authConfig, WorldState state)
        {
            _tableClient = factory.GetCloudTable();
            _authConfig = authConfig.Value;
            _state = state;
        }

        public async Task<string> Create(string username, string password)
        {
            if (password.Length < 8)
            {
                return "Your password suck"; 
            }

            var table = _tableClient.GetTableReference("Users");
            var existingUser = table.CreateQuery<User>().Where(usr => usr.Username == username).ToList().SingleOrDefault();

            if (existingUser != null)
            {
                return "Username already exist";
            }

            await table.ExecuteAsync(TableOperation.Insert(new User(username, password)));

            return "created";
        }

        public string Authenticate(string username, string password)
        {
            var table = _tableClient.GetTableReference("Users");
            var user = table.CreateQuery<User>().Where(usr => usr.Username == username && usr.Password == password).ToList().SingleOrDefault(); // TODO : salt and pepper

            return user == null ? string.Empty : GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void Login(string username)
        {
            var player = (Player)_state.Actors.SingleOrDefault(act => act.Id == username);

            if (player == null)
            {
                // TODO : HANDLE FIRST LOGIN!!!!!!!!!!!!!
                var mapId = "testmap";
                player = new Player(username) { SpriteId = "player", Position = new Point(16, 12), MapId = mapId };
                _state.Actors.Add(player);
            }

            _state.ConnectedPlayers.Add(player);
        }

        public void Disconnect(string username)
        {
            var player = (Player)_state.Actors.Single(act => act.Id == username);
            _state.ConnectedPlayers.Remove(player);
        }
    }
}
