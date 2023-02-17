using AutoMapper;
using CoreBankingApi2.DTOs;
using CoreBankingApi2.Models;

namespace CoreBankingApi2.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
        }
      
    }
}
