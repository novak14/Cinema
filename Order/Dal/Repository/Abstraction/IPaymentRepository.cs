using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface IPaymentRepository
    {
        /// <summary>
        /// Prida polozku do payment a vrati ji
        /// </summary>
        /// <param name="IdMethod"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        Payment Add(int IdMethod, decimal Price);
    }
}
