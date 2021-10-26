using Contracts.Models.Request;
using Contracts.Models.Response;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contracts.Enums;

namespace Domain.Services
{
    public class TransactionsService : ITransactionsService
    {
              
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly ICurrentAccountRepository _currentAccountRepository;
        private readonly IUsersRepository _usersRepository;

         public TransactionsService (ITransactionsRepository transactionsRepository,
                      ICurrentAccountRepository currentAccountRepository,
                      IUsersRepository usersRepository)
                  {
                      _transactionsRepository = transactionsRepository;
                      _currentAccountRepository = currentAccountRepository;
                      _usersRepository = usersRepository;
                  }

        public async Task<TransactionResponse> TopUp(TransactionRequest request, string firebaseId)
        {
        var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseId);
        var account = await _currentAccountRepository.GetAsync(request.AccountId);
        if (account.Balance + request.Amount < 0)
            {
            throw new Exception($"   NOT ENOUGH FUNDS TO WRITE OFF   ");
            }
        var transactionWriteModels = new TransactionWriteModel
            {
             Id = Guid.NewGuid(),
             UserId = user.UserId,
             AccountId = request.AccountId,
             TransactionType = request.TransactionType,
             Amount = request.Amount,
             Comment = request.Comment,
             DateCreated = DateTime.Now
             };

         var currentAccountWriteModels = new CurrentAccountWriteModel
            {
             Id = account.Id,
             UserId = account.UserId,
             Balance = account.Balance + request.Amount,
             Currency = account.Currency,
             DateCreated = account.DateCreated
             };

         await _transactionsRepository.SaveOrUpdateAsync(transactionWriteModels);
         await _currentAccountRepository.SaveOrUpdateAsync(currentAccountWriteModels);

         return new TransactionResponse
             {
              Id = transactionWriteModels.Id,
              UserId = transactionWriteModels.UserId,
              TransactionType = transactionWriteModels.TransactionType,
              Amount = transactionWriteModels.Amount,
              Comment = transactionWriteModels.Comment,
              DateCreated = transactionWriteModels.DateCreated
             };
         }
         public Task<TransactionResponse> Receive(TransactionRequest request)
         {

            throw new NotImplementedException();

        }
        public async Task<SendTransactionResponse> Send(SendTransactionRequest request, string firebaseId)
        {
            var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseId);
            var account = await _currentAccountRepository.GetAsync(request.AccountId);
            var receiverAccount = await _currentAccountRepository.GetAsync(request.ReceiverSenderAccountId);

            if (account.Balance < request.Amount)
            {
                throw new Exception($"   NOT ENOUGH FUNDS TO WRITE OFF   ");
            }
/*            if (account.Currency != receiverAccount.Currency)
            {
                throw new Exception($"try another currency");
            }*/
            var senderTransactionWriteModels = new TransactionWriteModel
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                AccountId = request.AccountId,
                ReceiverAccountId = receiverAccount.Id,
                TransactionType = TransactionType.Send,
                Amount = -request.Amount,
                Comment = request.Comment,
                DateCreated = DateTime.Now
            };
            var receiverTransactionWriteModels = new TransactionWriteModel
            {
                Id = Guid.NewGuid(),
                UserId = receiverAccount.UserId,
                AccountId = receiverAccount.Id,
                ReceiverAccountId = request.AccountId,
                TransactionType = TransactionType.Receive,
                Amount = request.Amount,
                Comment = request.Comment,
                DateCreated = DateTime.Now
            };

            var accountWriteModels = new CurrentAccountWriteModel
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance - request.Amount,
                Currency = account.Currency,
                DateCreated = account.DateCreated
            };
            var receiverAccountWriteModels = new CurrentAccountWriteModel
            {
                Id = receiverAccount.Id,
                UserId = receiverAccount.UserId,
                Balance = receiverAccount.Balance + request.Amount,
                Currency = receiverAccount.Currency,
                DateCreated = receiverAccount.DateCreated
            };

            await _transactionsRepository.SaveOrUpdateAsync(senderTransactionWriteModels);
            await _transactionsRepository.SaveOrUpdateAsync(receiverTransactionWriteModels);
            await _currentAccountRepository.SaveOrUpdateAsync(accountWriteModels);
            await _currentAccountRepository.SaveOrUpdateAsync(receiverAccountWriteModels);

            return new SendTransactionResponse
            {
                Id = senderTransactionWriteModels.Id,
                UserId = senderTransactionWriteModels.UserId,
                ReceiverAccountId = senderTransactionWriteModels.ReceiverAccountId,
                TransactionType = senderTransactionWriteModels.TransactionType,
                Amount = senderTransactionWriteModels.Amount,
                Comment = senderTransactionWriteModels.Comment,
                DateCreated = senderTransactionWriteModels.DateCreated
            };
        }

        public async Task<ActionResult<TransactionResponse>> GetHistory(string firebaseId)
        {
            throw new NotImplementedException();
            var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseId);
            var transactions = await _transactionsRepository.GetAllAsync(user.UserId);
/*            return transactions.Select(transaction => new TransactionResponse
            {
               Id = transaction.Id,
               UserId = transaction.UserId,
               TransactionType = transaction.TransactionType,
               Amount = transaction.Amount,
               Comment = transaction.Comment,
               DateCreated = transaction.DateCreated
             });*/
        }
        public async Task<ActionResult<TransactionResponse>> GetHistoryByDate()
        {

            throw new NotImplementedException();
        }

   
    }
}

