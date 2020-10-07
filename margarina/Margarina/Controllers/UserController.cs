using System.Threading.Tasks;
using Margarina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Margarina.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public UserController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("authentication")]
        public IActionResult Authenticate(UserRequest request)
        {
            var token = _playerService.Authenticate(request.Username, request.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(new AuthenticationResponse { Token = token });
           
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRequest request)
        {
            // TODO temporary stuff
            var message = await _playerService.Create(request.Username, request.Password);

            if (message == "created")
            {
                return Ok(message);
            }

            return BadRequest(message);
        }
    }
}
