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
    public class UsersRepository : IUsersRepository
    {
        private const string TableName = "Users";
        private readonly ISqlClient _sqlClient;

        public UsersRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }
        public Task<int> CreateUserAsync(UserWriteModel model)
        {
            var sql = @$"INSERT INTO {TableName} (UserId, FirebaseId, UserName, CurrentAccount, Balance, Tranaction, Deposit, Shares) 
                        VALUES (@UserId, @FirebaseId, @Username, @CurrentAccount, @Balance, @Tranaction, @Deposit, @Shares)";

            return _sqlClient.ExecuteAsync(sql, model);

        }
     
        public Task<UserReadModel> GetUserByFirebaseIdAsync(string firebaseId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE FirebaseId = @FirebaseId";

            return _sqlClient.QuerySingleOrDefaultAsync<UserReadModel>(sql, new
            {
                FirebaseId = firebaseId
            });
        }
        public Task<int> DeleteUserByIdAsync(Guid userId)
        {
            var sql = $"DELETE FROM {TableName} WHERE UserId = @UserId";

            return _sqlClient.ExecuteAsync(sql, new
            {
                UserId = userId
            });
        }

    }
}
