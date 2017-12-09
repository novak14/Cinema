using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface INewOrderRepository
    {
        /// <summary>
        /// Pridani objednavky
        /// </summary>
        /// <param name="IdUser"></param>
        /// <param name="IdPayment"></param>
        /// <param name="IdDeliveryType"></param>
        NewOrder Add(string IdUser, int IdPayment, int IdDeliveryType);
    }
}
