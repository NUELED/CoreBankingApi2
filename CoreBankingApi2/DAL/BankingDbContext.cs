using CoreBankingApi2.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBankingApi2.DAL
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)  
        {

        }

        public DbSet<Account> Accounts { get; set; }    
        public DbSet<Transaction> Transactions { get; set; }    
    }
}
