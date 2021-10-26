using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Enums;

namespace Contracts.Models.Response
{
    public class CurrentAccountResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<TransactionResponse> Transactions { get; set; }
    }
}
