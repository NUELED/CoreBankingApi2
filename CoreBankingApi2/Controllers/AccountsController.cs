using AutoMapper;
using CoreBankingApi2.DTOs;
using CoreBankingApi2.Logging;
using CoreBankingApi2.Models;
using CoreBankingApi2.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace CoreBankingApi2.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        //private readonly ILogger<AccountsController> _logger;   
        private readonly ILogging _logger;   

        public AccountsController(IAccountService accountService, IMapper mapper,/* ILogger<AccountsController> logger*/ ILogging logger)
        {
            _accountService = accountService;
            _mapper = mapper;   
           // _logger = logger;   
            _logger = logger;   
        }




        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)   
        {
            if(!ModelState.IsValid) return BadRequest(newAccount);
            
            //map to account
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfirmPin)); 
        }



        [HttpGet]
        [Route("get_all_accounts")]
        public IActionResult GetAllAccounts()
        {
            try
            {
                // _logger.LogInformation("Fetching all accounts from the database"); // this is for the custom logger "Ilogger"
                _logger.Information("Fetching all accounts from the database ##################***********", "Information"); //

                var accounts = _accountService.GetAllAccounts();

               // throw new Exception("An issue occured while getting all the accounts from the database");
                var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);

                

                //  _logger.LogInformation("Returning all the accounts from the database"); // this is for the custom logger "Ilogger"
                _logger.Log("Returning all the accounts from the database","warning");
                return Ok(cleanedAccounts);
            }
            catch (Exception ex)
            {

                // _logger.LogError($"Something went wrong: {ex}");  // this is for the custom logger  "Ilogger"
                _logger.Log($"Something went wrong: {ex}", "error");
                return StatusCode(500, "Internal server Error");
            }
            
           
        }



        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Authenticatet([FromBody] AuthenticateModel model)
        {

            if (!ModelState.IsValid) return BadRequest(model);

            return Ok(_accountService.Authenticate(model.AccountNumber, model.Pin)); // this might be mapped
        }


        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10-digit");
            var account = _accountService.GetByAccountNumber(AccountNumber);    
            var cleanedAccount = _mapper.Map<GetAccountModel>(account); 

            return Ok(cleanedAccount);
        }

        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int Id)
        {
           
            var account = _accountService.GetById(Id);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);

            return Ok(cleanedAccount);
        }


        [HttpPut]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
        {

            if (!ModelState.IsValid) return BadRequest(model);

            var account = _mapper.Map<Account>(model);

            _accountService.Update(account, model.Pin); 
            return Ok(); 
        }

    }
}
