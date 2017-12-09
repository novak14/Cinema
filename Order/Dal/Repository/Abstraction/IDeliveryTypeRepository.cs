using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface IDeliveryTypeRepository
    {
        /// <summary>
        /// Ziska vsechny zpusoby prevzeti
        /// </summary>
        /// <returns></returns>
        List<DeliveryType> GetAllMethod();
    }
}
