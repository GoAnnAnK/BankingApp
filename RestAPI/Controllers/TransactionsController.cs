using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("Transactions")]
    
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IUsersRepository _usersRepository;
        public TransactionsController(ITransactionsService transactionsService, ITransactionsRepository transactionsRepository, IUsersRepository usersRepository)
        {
            _transactionsService = transactionsService;
            _transactionsRepository = transactionsRepository;
            _usersRepository = usersRepository;
        }

        
        [HttpPost]
        [Authorize]
        [Route("TopUp")]
        public async Task<ActionResult<TransactionResponse>> TopUpBalance(TransactionRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseId);
            try
            {
                var response = await _transactionsService.TopUp(request, firebaseId);

                return response;
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            };
        }
        
            // POST api/<TransactionsController>
        [HttpPost]
        [Authorize]
        [Route("TransactionHistory")]
        public async Task<ActionResult<TransactionResponse>> GetHistory(Guid userId) 
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseId);
            var transactions = await _transactionsRepository.GetAllAsync(user.UserId);
            try
            {
                var response =  transactions.Select(transaction => new TransactionResponse
                {
                    Id = transaction.Id,
                    UserId = transaction.UserId,
                    Amount = transaction.Amount,
                    Comment = transaction.Comment,
                    DateCreated = transaction.DateCreated
                });
                return Ok(response);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
/*            var transaction = await _transactionsRepository.GetAllAsync(userId);
            var response = transaction.Select( transaction => new TransactionResponse
            {
                Id = transaction.Id,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                Comment = transaction.Comment,
                DateCreated = transaction.DateCreated,
            });
            return Ok(response);*/
        }
        [HttpPost]
        [Authorize]
        [Route("Send")]
        public async Task<ActionResult<SendTransactionResponse>> Transaction(SendTransactionRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            var user = await _usersRepository.GetUserByFirebaseIdAsync(firebaseId);

            try
            {
                var response = await _transactionsService.Send(request, firebaseId);

                return response;
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
