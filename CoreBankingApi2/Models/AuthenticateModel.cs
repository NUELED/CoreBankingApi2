using System.ComponentModel.DataAnnotations;

namespace CoreBankingApi2.Models
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$")]
        public string AccountNumber { get; set; }

        [Required]
        public string Pin { get; set; }    

    }
}
