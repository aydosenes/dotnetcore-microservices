using Market.Core.Data.Entities;
using MarketMicroservice.Data;
using MarketMicroservice.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Business.Services
{
    public class SaleService : ISaleService
    {
        private AppDbContext _dbContext;

        public SaleService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDataResult<IEnumerable<Bill>>> Billing(List<string> CodeList)
        {
            try
            {
                var _bill = new Bill();
                object[] bill = new object[_bill.GetType().GetProperties().Length];

                _bill.Date = DateTime.UtcNow + TimeSpan.FromHours(3);
                _bill.IsCash = true;
                _bill.Sum = 0.0;
                for (int i = 0; i <= CodeList.Count; i++)
                {
                    var product = await _dbContext.Products.Where(x => x.Code == CodeList.ElementAt(i)).FirstOrDefaultAsync();
                    if (product != null && product.StockAmount != 0)
                    {
                        _bill.Products.Add(product);
                        _bill.Sum += product.Price;
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        return new FailDataResult<IEnumerable<Bill>>("Faturalanacak ürün bulunamadı.");
                    }
                }

                for (int i = 0; i <= _bill.GetType().GetProperties().Length; i++)
                {
                    bill[i] = _bill.GetType().GetProperties().GetValue(i).ToString();
                }

                var result = ((IEnumerable)bill).Cast<Bill>().ToList();

                return new SuccessDataResult<IEnumerable<Bill>>(result);
            }
            catch (Exception ex)
            {
                return new FailDataResult<IEnumerable<Bill>>("Faturalama Hatasi." + ex.Message.ToString());
            }

        }
    }
}
