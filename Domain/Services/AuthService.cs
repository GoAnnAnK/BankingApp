using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Clients.Firebase.Options;
using Microsoft.Extensions.Options;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Persistence.Repositories;
using Domain.Clients.Firebase;
using Persistence.Models;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;

namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IFirebaseClient _firebaseClient;
        private readonly IUsersRepository _usersRepository;

        public AuthService(IFirebaseClient firebaseClient, IUsersRepository usersRepository)
        {
            _firebaseClient = firebaseClient;
            _usersRepository = usersRepository;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = await _firebaseClient.RegisterAsync(request.Email, request.Password);

            var userWriteModel = new UserWriteModel
            {
                UserId = Guid.NewGuid(),
                FirebaseId = user.FirebaseId,
                Username = request.Username,

            };

            await _usersRepository.CreateUserAsync(userWriteModel);

            return new RegisterResponse
            {
                Id = userWriteModel.UserId,
                IdToken = user.IdToken,
                Email = userWriteModel.Email,
                Username = userWriteModel.Username,
                
            };
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var firebaseSignInResponse = await _firebaseClient.LoginAsync(request.Email, request.Password);

            var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseSignInResponse.FirebaseId);

            return new LoginResponse
            {
                Username = user.Username,
                Email = user.Email,
                IdToken = firebaseSignInResponse.IdToken
            };
        }




        
    }
}

