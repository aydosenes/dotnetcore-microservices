using Market.Core.Data.Entities;
using MarketMicroservice.Data;
using MarketMicroservice.Data.Entities;
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

        public async Task<IDataResult<Bill>> Billing(List<string> CodeList)
        {
            try
            {
                var _bill = new Bill();
                //var _product = new Product();
                object[] bill = new object[_bill.GetType().GetProperties().Length];
                Product[] codes = new Product[CodeList.Count];

                _bill.Date = DateTime.UtcNow + TimeSpan.FromHours(3);
                _bill.IsCash = true;
                _bill.Sum = 0.0;
                for (int i = 0; i < CodeList.Count; i++)
                {
                    var product = await _dbContext.Products.Where(x => x.Code == CodeList.ElementAt(i)).FirstOrDefaultAsync();
                    if (product != null)
                    {
                        codes[i] = product;
                        _bill.Sum += product.Price;

                    }
                    else
                    {
                        return new FailDataResult<Bill>("Faturalanacak ürün bulunamadı.");
                    }
                }
                if(codes != null)
                {
                    _bill.Products = codes.ToList();
                    await _dbContext.SaveChangesAsync();

                }
                //for (int i = 0; i < bill.Length; i++)
                //{
                //    bill[i] = _bill.GetType().GetProperties().GetValue(i);
                //}

                //((IEnumerable)bill).Cast<Bill>().ToList();

                return new SuccessDataResult<Bill>(_bill);
            }
            catch (Exception ex)
            {
                return new FailDataResult<Bill>("Faturalama Hatasi." + ex.Message.ToString());
            }

        }
    }
}
