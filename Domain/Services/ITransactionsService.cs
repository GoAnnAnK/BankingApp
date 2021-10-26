using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Clients.Firebase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Services
{
    public interface ITransactionsService
    {

        Task<TransactionResponse> TopUp(TransactionRequest request, string firebaseId);
        Task<SendTransactionResponse> Send(SendTransactionRequest request, string firebaseId);
        Task<TransactionResponse> Receive(TransactionRequest request);
        Task<ActionResult<TransactionResponse>> GetHistory(string firebaseId);
        Task<ActionResult<TransactionResponse>> GetHistoryByDate();
    }
}
