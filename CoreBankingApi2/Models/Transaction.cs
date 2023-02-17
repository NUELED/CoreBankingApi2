using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBankingApi2.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }        // This will generate in every instance of the class.
        public decimal TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public bool Issuccessfull => TransactionStatus.Equals(TranStatus.Success);  
        public string TransactionSourceAccount { get; set; }   
        public string TransactionDestinationAccount { get; set; }   
        public string TransactionParticulars { get; set; }   
        public TranType TransactionType { get; set; }   
        public DateTime TransactionDate { get; set; }

        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-","").Substring(1, 27)}";
        }
    }

    public enum TranStatus
    {
        Failed,
        Success, 
        Error
    }

    public enum TranType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
