using Market.Core.Data.Entities;
using MarketMicroservice.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Market.Business.Services
{
    public interface ISaleService
    {
        Task<IDataResult<Bill>> Billing(List<string> CodeList);
    }
}
