using System;
using System.Threading.Tasks;
using Margarina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Margarina.Controllers
{
    [Route("user/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public AuthenticationController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticationRequest request)
        {
            try
            {
                var token = await _playerService.Authenticate(request.Username, request.Password);
                return Ok(new AuthenticationResponse { Token = token });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
