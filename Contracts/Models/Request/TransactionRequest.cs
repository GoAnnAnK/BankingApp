using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Enums;

namespace Contracts.Models.Request
{
    public class TransactionRequest
    {
        public Guid AccountId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
