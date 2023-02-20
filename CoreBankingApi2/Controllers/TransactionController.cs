using AutoMapper;
using CoreBankingApi2.DTOs;
using CoreBankingApi2.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
//using System.Transactions;
using CoreBankingApi2.Models;
using CoreBankingApi2.Services.Implementations;

namespace CoreBankingApi2.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private /*readonly*/ ITransactionService _transactionService;
        IMapper _mapper;


        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;   
        }




        [HttpPost]
        [Route("create_new_transaction")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDto transactionRequest)
        {
            // lets create a transactionRequestDto and map to transaction.The above will be done in the automapperprofile class
            if (!ModelState.IsValid) return BadRequest(transactionRequest);


            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }





        [HttpPost]
        [Route("make_deposit")]
        public IActionResult MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digit");

            //(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$")

            return Ok(_transactionService.MakeDeposit(AccountNumber, Amount, TransactionPin));
        }





        [HttpPost]
        [Route("make_withdrawal")]
        public IActionResult MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digit");

            return Ok(_transactionService.MakeWithdrawal(AccountNumber, Amount, TransactionPin));
        }




        [HttpPost]
        [Route("make_funds_transfer")]
        public IActionResult MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            if (!Regex.IsMatch(FromAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
                return BadRequest("Account number must be 10-Digit");

            return Ok(_transactionService.MakeFundsTransfer(FromAccount, ToAccount, Amount, TransactionPin));
        }


        //[HttpGet]
        //[Route("get_transaction_by_date")]
        //public IActionResult  GetTransactionByDate(DateTime transDate)
        //{

        //    var date = _transactionService.FindTransactionByDate(transDate);
        //    //var cleanedAccount = _mapper.Map<GetAccountModel>(account);

        //    return Ok(date);
        //}
    }
}
