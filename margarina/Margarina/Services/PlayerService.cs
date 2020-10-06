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
using Margarina.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Margarina.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly MargarinaContext _context;
        private readonly AuthenticationConfig _authConfig;
        private readonly WorldState _state;

        public PlayerService(MargarinaContext context, IOptions<AuthenticationConfig> authConfig, WorldState state)
        {
            _context = context;
            _authConfig = authConfig.Value;
            _state = state;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(usr => usr.Username == username && usr.Password == password); // TODO : salt and pepper

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
