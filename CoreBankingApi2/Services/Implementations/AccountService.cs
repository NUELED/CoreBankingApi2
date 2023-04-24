using CoreBankingApi2.DAL;
using CoreBankingApi2.Models;
using CoreBankingApi2.Services.Interfaces;
using System.Net.NetworkInformation;
using System.Text;

namespace CoreBankingApi2.Services.Implementations
{
    public class AccountService : IAccountService
    {

        private readonly BankingDbContext _dbContext;
        public AccountService(BankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public Account Authenticate(string AccountNumber, string Pin)
        {
            //Yolls, lets Authenticate
            //var account = _dbContext.Accounts.where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault; //This can be used too
            var account = _dbContext.Accounts.SingleOrDefault(x=> x.AccountNumberGenerated == AccountNumber); //veryfing account
            if (account == null)
                return null;

            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;

            //account has been authenticated!
            return account;
        }



        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)   //This private static method verifies the pin.
        {
            if (string.IsNullOrEmpty(Pin)) throw  new  ArgumentNullException("Pin");

            //Verifying pin
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
               var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] == pinHash[i]) return true;
                }
            }

            return true;    
        }



        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            if (_dbContext.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An account already exixt with this email");
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pins do not match","Pin");

            // We are hashing/encrypting pin below. The "out" keyword enables you to retuen more than value back in a function.
            // I.e The other values you want to return is passed as a parameter in the function with the out keyword attached.
            byte[] pinHash, pinSalt;

            CreatePinHash(Pin, out pinHash, out pinSalt); //This crypto/encrypting  method is created below.
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;  

            _dbContext.Accounts.Add(account);  //Here, new account is added to Db.
            _dbContext.SaveChanges();

            return account; 
        }



        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt) //The encryotion function
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinHash = hmac.Key;
                pinSalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));     
            }
        }


        public void Delete(int Id)
        {
            var account = _dbContext.Accounts.Find(Id); 
            if(account != null)
            {
                _dbContext.Accounts.Remove(account);
                _dbContext.SaveChanges();
            }
        }


        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
        }


        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.AccountNumberGenerated == AccountNumber);
            if (null == account) return null;
            return account;
        }

        public Account GetById(int Id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.Id == Id);
            if (null == account) return null;
            return account;
        }

        public void Update(Account account, string Pin = null)
        {
            var accountToBeUpdated = _dbContext.Accounts.SingleOrDefault( x => x.Email.ToLower() == account.Email.ToLower() );
            if (accountToBeUpdated == null) throw new ApplicationException("Account does not exist");
            // If the account exist...then let's do the magic.

            if(!string.IsNullOrWhiteSpace(account.Email))
            {
                // It implies that the user wants to change their email
                // There is an issue in this update method. We'll use id instead of email to fix it later.
                if (_dbContext.Accounts.Any(x => x.Email.ToLower() == account.Email.ToLower()) != null)
                {
                    throw new ApplicationException  ("This email" + account.Email + "already exists"); // We'll use id to look in the error thrown here instead of email.
                    accountToBeUpdated.Email = account.Email;
                }
                  
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                // It implies that the user wants to change their phonenumber
                if (_dbContext.Accounts.Any(x => x.Email == account.PhoneNumber)) throw new ApplicationException
                   ("This phonenumber" + account.PhoneNumber + "already exists");
                   accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }


            if (!string.IsNullOrWhiteSpace(Pin))
            {
                // It implies that the user wants to change their pin
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt); 

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;
            }
            accountToBeUpdated.DateLastUpdated = DateTime.Now;
              //Here we sending the changes to the db
              _dbContext.Accounts.Update(accountToBeUpdated);
              _dbContext.SaveChanges();
        }
    }
}
