using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface IPaymentMethodRepository
    {
        /// <summary>
        /// Ziska vsechny zpusoby platby
        /// </summary>
        /// <returns></returns>
        List<PaymentMethod> GetAllMethod();
    }
}
