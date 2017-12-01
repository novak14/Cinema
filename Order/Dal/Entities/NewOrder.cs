using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Entities
{
    public class NewOrder
    {
        public int IdOrder { get; set; }
        public string IdUser { get; set; }
        public int IdPayment { get; set; }
        public int IdDeliveryType { get; set; }
    }
}
