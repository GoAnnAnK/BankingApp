using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Clients.Firebase.Models;

namespace Domain.Clients.Firebase
{
    public interface IFirebaseClient
    {
        Task<FirebaseRegisterResponse> RegisterAsync(string email, string password);

        Task<FirebaseLoginResponse> LoginAsync(string email, string password);
    }
}
