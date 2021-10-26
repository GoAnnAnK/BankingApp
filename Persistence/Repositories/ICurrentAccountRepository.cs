using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ICurrentAccountRepository
    {
        Task<IEnumerable<CurrentAccountReadModel>> GetAllAsync(Guid userId);

        Task<CurrentAccountReadModel> GetAsync(Guid id);

        Task<int> SaveOrUpdateAsync(CurrentAccountWriteModel model);

        Task<int> DeleteAsync(Guid id);
    }
}
