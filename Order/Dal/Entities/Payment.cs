using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Entities
{
    public class Payment
    {
        public int IdPayment { get; set; }
        public decimal Price { get; set; }
        public int IdMethod { get; set; }
    }
}
