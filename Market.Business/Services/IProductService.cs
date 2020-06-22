﻿using Market.Core.Repository;
using MarketMicroservice.Business.Models;
using MarketMicroservice.Data.Entities;
using MarketMicroservice.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMicroservice.Business.Services
{
    public interface IProductService //: IRepositoryBase<Product>
    {
        Task<IDataResult<IEnumerable<Product>>> GetProducts();
        Task<IDataResult<Product>> GetProductById(int id);
        Task<IDataResult<Product>> AddProduct(ProductModel product);
        Task<IDataResult<Product>> BuyProduct(List<string> CodeList);
        Task<IDataResult<Product>> UpdateProduct(int id, ProductModel product);
        Task<IDataResult<Product>> UpdateStock(int id, int stock);
        Task<IDataResult<Product>> DeleteProduct(int id);


    }
}