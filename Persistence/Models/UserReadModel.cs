using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models
{
    public class UserReadModel
    {
        public Guid Id { get; set; }
        public string FirebaseId { get; set; }
        public decimal CurrentAccount { get; set; }
        public decimal Balance { get; set; }
        public string Transaction { get; set; }
        public decimal Deposit { get; set; }
        public decimal Shares { get; set; }

    }
}
