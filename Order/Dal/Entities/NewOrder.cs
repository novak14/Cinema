using Catalog.Dal.Entities;
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
        public DateTime CreateDate { get; set; }

        public Payment Payment { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public Film Film { get; set; }
        public OrderFilm OrderFilm { get; set; }
        public List<CartPlaces> CartPlaces { get; set; } = new List<CartPlaces>();
    }
}
