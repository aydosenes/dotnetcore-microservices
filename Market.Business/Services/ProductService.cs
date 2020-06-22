using Market.Business.Services;
using MarketMicroservice.Business.Models;
using MarketMicroservice.Data;
using MarketMicroservice.Data.Entities;
using MarketMicroservice.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMicroservice.Business.Services
{
    public class ProductService : IProductService
    {
        private AppDbContext _dbContext;
        private ISaleService _saleService;

        public ProductService(AppDbContext dbContext, ISaleService saleService)
        {
            _dbContext = dbContext;
            _saleService = saleService;
        }

        public async Task<IDataResult<Product>> AddProduct(ProductModel product)
        {
            try
            {
                var _product = new Product();
                _product.Code = product.Code;
                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.InsertionDate = DateTime.UtcNow + TimeSpan.FromHours(3);
                _product.UpdatedDate = DateTime.UtcNow + TimeSpan.FromHours(3);
                _product.IsRejected = false;
                _product.StockAmount = product.StockAmount;
                var result = await _dbContext.Products.AddAsync(_product);
                await _dbContext.SaveChangesAsync();
                return new SuccessDataResult<Product>(_product);

            }
            catch (Exception ex)
            {
                return new FailDataResult<Product>("Ürün eklenemedi." + ex.Message.ToString());
            }
        }

        public async Task<IDataResult<Product>> BuyProduct(List<string> CodeList)
        {
            try
            {
                List<string> OutOfStock = new List<string>();
                double price = 0.0;
                bool isNotNull = CodeList != null ? true : false;
                if (isNotNull)
                {
                    for (int i = 0; i <= CodeList.Count; i++)
                    {
                        var product = await _dbContext.Products.Where(x => x.Code == CodeList.ElementAt(i)).FirstOrDefaultAsync();
                        if (product != null && product.StockAmount != 0)
                        {
                            product.StockAmount--;
                            product.UpdatedDate = DateTime.UtcNow + TimeSpan.FromHours(3);
                            price += product.Price;
                            var billResult = _saleService.Billing(CodeList);
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            OutOfStock.Add(product.Code.ToString());
                        }
                    }
                    if (OutOfStock == null)
                    {
                        return new SuccessDataResult<Product>("Satış gerçekleşti. Toplam fiyat: " + price.ToString());
                    }
                    else
                        return new FailDataResult<Product>(OutOfStock.ToList() + ": Bu ürünler stokta yok.");

                }
                return new FailDataResult<Product>("Liste boş.");
            }
            catch (Exception ex)
            {
                return new FailDataResult<Product>("Satış gerçekleşmedi. " + ex.Message.ToString());
            }
        }

        public async Task<IDataResult<Product>> DeleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.Products
                                        .Where(x => x.ProductId == id)
                                        .FirstOrDefaultAsync();
                product.StockAmount--;
                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();

                return new SuccessDataResult<Product>(product);
            }
            catch (Exception ex)
            {
                return new FailDataResult<Product>("Ürün silinemedi." + ex.Message.ToString());
            }

        }

        public async Task<IDataResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _dbContext.Products
                                        .Where(x => x.ProductId == id)
                                        .FirstOrDefaultAsync();

                return new SuccessDataResult<Product>(product);
            }
            catch (Exception ex)
            {
                return new FailDataResult<Product>("Listelenecek ürün bulunamadı." + ex.Message.ToString());
            }
        }

        public async Task<IDataResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var result = await _dbContext.Products.ToListAsync();

                return new SuccessDataResult<IEnumerable<Product>>(result);
            }
            catch (Exception ex)
            {
                return new FailDataResult<IEnumerable<Product>>("Listelenecek ürün bulunamadı." + ex.Message.ToString());
            }
        }

        public async Task<IDataResult<Product>> UpdateProduct(int id, ProductModel product)
        {
            try
            {
                var _product = await _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
                _product.Code = product.Code;
                _product.Name = product.Name;
                _product.Price = product.Price;
                _product.UpdatedDate = DateTime.UtcNow + TimeSpan.FromHours(3);
                _product.IsRejected = product.IsRejected;

                //_dbContext.Update(_product);
                await _dbContext.SaveChangesAsync();

                return new SuccessDataResult<Product>(_product);
            }
            catch (Exception ex)
            {
                return new FailDataResult<Product>("Ürün güncellenemedi." + ex.Message.ToString());
            }
        }

        public async Task<IDataResult<Product>> UpdateStock(int id, int stock)
        {
            try
            {
                var _product = await _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
                _product.StockAmount = stock;

                await _dbContext.SaveChangesAsync();

                return new SuccessDataResult<Product>(_product);
            }
            catch (Exception ex)
            {
                return new FailDataResult<Product>("Stok sayısı güncellenemedi." + ex.Message.ToString());
            }
        }
    }
}
