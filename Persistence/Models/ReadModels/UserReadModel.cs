using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class UserReadModel
    {
        public Guid UserId { get; set; }
        public string FirebaseId { get; set; }
        public string Username { get; set; }
        public decimal CurrentAccount { get; set; }
        public decimal Balance { get; set; }
        public string Transaction { get; set; }
        public decimal Deposit { get; set; }
        public decimal Shares { get; set; }
        public string Email { get; set; }
    }
}
