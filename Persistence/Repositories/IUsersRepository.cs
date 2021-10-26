using Persistence.Models;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IUsersRepository
    {
        Task<int> CreateUserAsync(UserWriteModel model);

        Task<UserReadModel> GetUserByFirebaseIdAsync(string firebaseId);

        Task<int> DeleteUserByIdAsync(Guid userId);
    }
}
