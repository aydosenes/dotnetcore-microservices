﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMicroservice.Business.Models
{
    public class ProductModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsRejected { get; set; }
        public int StockAmount { get; set; }

    }
}
