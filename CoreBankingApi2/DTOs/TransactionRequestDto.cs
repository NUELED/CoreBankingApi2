using CoreBankingApi2.Models;
using System.ComponentModel.DataAnnotations;

namespace CoreBankingApi2.DTOs
{
    public class TransactionRequestDto
    {

        [Key]
        public int Id { get; set; }
        public decimal TransactionAmount { get; set; }
       
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        public TranType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
