using Domain.Clients.Firebase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Domain.Services;
using Contracts.Models.Response;
using Contracts.Models.Request;
using Domain.Exceptions;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);

                return response;
            }

            catch (FirebaseException e)
            {
                return BadRequest(e.Message);
            }
        }
         
         [HttpPost]
         [Route("Login")]
         public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
         {
            try
            {
                var response = await _authService.LoginAsync(request);

                return response;
            }

            catch (FirebaseException e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
