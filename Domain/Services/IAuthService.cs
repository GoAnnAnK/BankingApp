using Contracts.Models.Request;
using Contracts.Models.Response;
using System;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);

    }
}
