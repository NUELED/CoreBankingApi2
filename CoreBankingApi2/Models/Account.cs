using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace CoreBankingApi2.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; } 
        public string FirstName { get; set; }    
        public string LastName { get; set; }    
        public string AccountName { get; set; }    
        public string PhoneNumber { get; set; }    
        public string Email { get; set; }    
        public decimal CurrentAccountBalance { get; set; }    
        public AccountType AccountType { get; set; }  
        public string AccountNumberGenerated { get; set; }
        [JsonIgnore]
        public byte[] PinHash { get; set; }  // hash of the account Transaction pin
        [JsonIgnore]
        public byte[] PinSalt { get; set; }   // Salt of the account Transaction pin
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        //We'll generate the account number in the constructor.......
        Random rand = new Random();  //Creates an instance of the Random class.
        public Account()
        {
            
            AccountNumberGenerated = Convert.ToString((long) Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
            //I used 9_000_000_000 so i can generate a 10 digit random number.
            AccountName = $"{FirstName}{LastName}";
        }
    }  

    public enum AccountType
    {
        Savings, // savings => 0, currrent => 1 e.t.c
        Current,
        Cooperate,
        Government
    }
}
