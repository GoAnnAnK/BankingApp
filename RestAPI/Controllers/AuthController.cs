using Domain.Clients.Firebase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Domain.Services;
using Contracts.Models.Response;
using Contracts.Models.Request;

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
                    var newUser = await _authService.RegisterAsync(request);

                    return Ok(newUser);
                }
         [Authorize]
         [HttpPost]
         [Route("Login")]
         public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
                {
                    var returnedUser = await _authService.LoginAsync(request);

                    return Ok(returnedUser);
                }

/*                [HttpPost]
                [Route("changePassword")]
                [Authorize]

                public async Task<ActionResult<EditUserResponse>> ChangePassword(ChangePasswordRequest request)
                {
                    Request.Headers.TryGetValue("Authorization", out var idToken);

                    //var idToken = this.GetHeaderData("Authorization");

                    var idTokenValue = idToken.First().Remove(0, 7); // removes 'Bearer ' from the header

                    var response = await _userService.ChangePasswordAsync(new ChangePasswordRequestModel
                    {
                        IdToken = idTokenValue,
                        NewPassword = request.NewPassword,
                        ReturnSecureToken = true
                    });

                    return Ok(response);
                }

                [HttpPost]
                [Route("changeEmail")]
                [Authorize]

                public async Task<ActionResult<EditUserResponse>> ChangeEmail(ChangeEmailRequest request)
                {
                    var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

                    if (userId is null)
                    {
                        return NotFound();
                    }

                    var user = await _userService.GetUserAsync(userId.Value);

                    Request.Headers.TryGetValue("Authorization", out var idToken);

                    var idTokenValue = idToken.ToString().Remove(0, 7); // removes 'Bearer ' from the header

                    var response = await _userService.ChangeEmailAsync(user.UserId, new ChangeEmailRequestModel
                    {
                        IdToken = idTokenValue,
                        NewEmail = request.NewEmail,
                        ReturnSecureToken = true
                    });

                    return Ok(response);
                }*/
    }
}
