using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMicroservice.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockAmount { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsRejected { get; set; }


    }
}
