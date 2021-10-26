using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CurrentAccountRepository : ICurrentAccountRepository
    {
        private const string TableName = "accounts";
        private readonly ISqlClient _sqlClient;

        public CurrentAccountRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<IEnumerable<CurrentAccountReadModel>> GetAllAsync(Guid userId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE UserId = @UserId";

            return _sqlClient.QueryAsync<CurrentAccountReadModel>(sql);
        }

        public Task<CurrentAccountReadModel> GetAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";

            return _sqlClient.QuerySingleOrDefaultAsync<CurrentAccountReadModel>(sql, new { Id = id });
        }

        public Task<int> SaveOrUpdateAsync(CurrentAccountWriteModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, UserId, Balance, Currency, DateCreated) 
                        VALUES (@Id, @UserId, @Balance, @Currency, @DateCreated)
                        ON DUPLICATE KEY UPDATE Balance = @Balance, Currency = @Currency";

            return _sqlClient.ExecuteAsync(sql, new
            {
                model.Id,
                model.UserId,
                model.Balance,
                Currency = model.Currency.ToString(),
                model.DateCreated
            });
        }
        public Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE from {TableName} WHERE Id = @Id";
            return _sqlClient.ExecuteAsync(sql, new { Id = id });
        }
    }
}
