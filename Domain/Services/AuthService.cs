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

            var userReadModel = new UserReadModel
            {
                Id = Guid.NewGuid(),
                FirebaseId = user.FirebaseId,
                Username = request.Username,

    };

            await _usersRepository.SaveAsync(userReadModel);

            return new RegisterResponse
            {
                Id = userReadModel.Id,
                IdToken = user.IdToken,
                Email = userReadModel.Email,
                Username = userReadModel.Username,
                
            };
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var firebaseSignInResponse = await _firebaseClient.LoginAsync(request.Email, request.Password);

            var user = await _usersRepository.GetAsync(firebaseSignInResponse.FirebaseId);

            return new LoginResponse
            {
                Username = user.Username,
                Email = user.Email,
                IdToken = firebaseSignInResponse.IdToken
            };
        }




        /*    public async Task<RegisterResponse> RegisterAsync(string email, string password)
            {
                var userReadModel = new UserReadModel
                {
                    Email = email,
                    Password = password,
                    ReturnSecureToken = true
                };
                var url = $"{_firebaseSettings.BaseAddress}/v1/accounts:signUp?key={_firebaseSettings.WebApiKey}";

                var response = await _httpClient.PostAsJsonAsync(url, userCreds);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CreateUserResponse>();
                }

                var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
            }

            public async Task<ClientSignInUserResponse> SignInUserAsync(string email, string password)
            {
                var url = $"{_firebaseSettings.BaseAddress}/v1/accounts:signInWithPassword?key={_firebaseSettings.WebApiKey}";

                var response = await _httpClient.PostAsJsonAsync(url, new FullSignInUserRequest
                {
                    Email = email,
                    Password = password,
                    ReturnSecureToken = true
                });

                if (response.IsSuccessStatusCode)
                {
                    return await
                        response.Content.ReadFromJsonAsync<ClientSignInUserResponse>();
                }

                var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
            }

            public async Task<ClientChangePasswordOrEmailResponse> ChangeUserPasswordAsync(ChangePasswordRequestModel request)
            {
                var url = $"{_firebaseSettings.BaseAddress}/v1/accounts:update?key={_firebaseSettings.WebApiKey}";

                var response = await _httpClient.PostAsJsonAsync(url, request);

                if (response.IsSuccessStatusCode)
                {
                    return await
                        response.Content.ReadFromJsonAsync<ClientChangePasswordOrEmailResponse>();
                }

                var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
            }

            public async Task<ClientChangePasswordOrEmailResponse> ChangeUserEmailAsync(ChangeEmailRequestModel request)
            {
                var url = $"{_firebaseSettings.BaseAddress}/v1/accounts:update?key={_firebaseSettings.WebApiKey}";

                var response = await _httpClient.PostAsJsonAsync(url, request);

                if (response.IsSuccessStatusCode)
                {
                    return await
                        response.Content.ReadFromJsonAsync<ClientChangePasswordOrEmailResponse>();
                }

                var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

                throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
            }*/
    }
}

