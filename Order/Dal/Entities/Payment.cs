using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Order.Dal.Entities
{
    public class Payment
    {
        [Key]
        public int IdPayment { get; set; }
        public decimal Price { get; set; }
        public int IdMethod { get; set; }
    }
}
