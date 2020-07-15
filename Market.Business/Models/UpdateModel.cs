using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Business.Models
{
    public class UpdateModel
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsRejected { get; set; }
        public int StockAmount { get; set; }

    }
}
