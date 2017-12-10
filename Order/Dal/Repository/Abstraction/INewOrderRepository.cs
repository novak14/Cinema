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
        NewOrder Add(string IdUser, int IdPayment, int IdDeliveryType, DateTime date);

        /// <summary>
        /// Ziska vsechny objednavky uzivatele
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        List<NewOrder> GetAllOrders(string IdUser);

        /// <summary>
        /// Ziska vsechny detaily k objednavce
        /// </summary>
        /// <param name="IdOrder"></param>
        /// <returns></returns>
        List<NewOrder> GetHistoryOrder(int IdOrder);
    }
}
