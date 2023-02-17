using CoreBankingApi2.Models;

namespace CoreBankingApi2.Services.Interfaces
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction); // There is a "Response" model,that handles responses
        Response FindTransactionByDate(DateTime date); 
        Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin); 
        Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin); 
        Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, int TransactionPin); 
    }
}
