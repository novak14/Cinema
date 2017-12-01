using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Entities
{
    public class PaymentMethod
    {
        public int IdMethod { get; set; }
        public string MethodType { get; set; }
    }
}
