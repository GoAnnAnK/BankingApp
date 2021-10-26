﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Enums;

namespace Contracts.Models.Response
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
