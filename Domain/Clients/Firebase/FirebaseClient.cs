using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Domain.Clients.Firebase.Models;
using Domain.Clients.Firebase.Options;
using Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace Domain.Clients.Firebase
{
    public class FirebaseClient : IFirebaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly FirebaseOptions _firebaseOptions;

        public FirebaseClient(HttpClient httpClient, IOptions<FirebaseOptions> firebaseOptions )
        {
            _httpClient = httpClient;
            _firebaseOptions = firebaseOptions.Value;
        }
        public async Task<FirebaseRegisterResponse> RegisterAsync(string email, string password)
        {
            var url = $"{_firebaseOptions.BaseAddress}//v1/accounts:signUp?key={_firebaseOptions.ApiKey}";
            var request = new FirebaseRegisterRequest

            {
                Email = email,
                Password = password
            };
            var response = await _httpClient.PostAsJsonAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FirebaseRegisterResponse>();
            }
            var firebaseError = await response.Content.ReadFromJsonAsync<ErrorResponse>();

            throw new FirebaseException(firebaseError.Error.Message, firebaseError.Error.StatusCode);
        }
        public async Task<FirebaseLoginResponse> LoginAsync(string email, string password)
        {
            var url = $"{_firebaseOptions.BaseAddress}/v1/accounts:signInWithPassword?key={_firebaseOptions.ApiKey}";

            var request = new FirebaseLoginRequest
            {
                Email = email,
                Password = password,

            };

            var response = await _httpClient.PostAsJsonAsync(url, request);

            return await response.Content.ReadFromJsonAsync<FirebaseLoginResponse>();
        }


    }
}
