using CoreBankingApi2.DAL;
using CoreBankingApi2.Models;
using CoreBankingApi2.Services.Interfaces;
using CoreBankingApi2.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreBankingApi2.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingDbContext _dbContext;   
        private readonly ILogger<TransactionService> _logger;
        private AppSettings _settings;
        public static string _ourBankSettlementAccount;
        private readonly IAccountService _accountService;   
       

        public TransactionService(BankingDbContext dbContext, ILogger<TransactionService> logger, IOptions<AppSettings> settings,
           IAccountService accountService)
        {

            _dbContext = dbContext; 
            _logger = logger;
            _settings = settings.Value;
            _ourBankSettlementAccount = _settings.OurBankSettlementAccount;
            _accountService = accountService;   
        }


        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successsfully";
            return response;    
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _dbContext.Transactions.Where(x => x.TransactionDate == date).ToList();   //cos of lots of daily transactions
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction created successsfully";
            response.Data = transaction;

            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //check that user account is valid
            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credential");


            //validation passes
            try
            {
                //For deposits, Our bankSettlementAccount is the source giving money to the user's account
                sourceAccount = _accountService.GetByAccountNumber(_ourBankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber); 

                //now let's update their account balances
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                // check for updates
                if((_dbContext.Entry(sourceAccount).State == EntityState.Modified) && 
                    (_dbContext.Entry(destinationAccount).State == EntityState.Modified))
                {

                    //transaction is successfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction successfull";
                    response.Data = null;


                }
                else
                {
                    //transaction is unsuccessfull
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction failled!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"AN ERROR OCCURED... => {ex.Message}");


            }

            // Set other props of transaction here
            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionSourceAccount = _ourBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;  
            transaction.TransactionAmount = Amount; 
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE =>{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} " +
                $"TO DESTINATION ACCOUNT => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $"ON DATE =>{JsonConvert.SerializeObject(transaction.TransactionDate)}" +
                $"FOR AMOUNT =>{JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"TRANSACTION TYPE =>{JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $"TRANSACTION STATUS =>{JsonConvert.SerializeObject(transaction.TransactionStatus)}";

              // lets save
              _dbContext.Transactions.Add(transaction);
              _dbContext.SaveChanges();

                return response;
                
        }





        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, int TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }
    }
}
