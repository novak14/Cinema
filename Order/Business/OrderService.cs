using Catalog.Dal.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Dal.Entities;

namespace Order.Business
{
    public class OrderService
    {
        private IFilmRepository _filmRepo;

        public OrderService(IFilmRepository filmRepo)
        {
            _filmRepo = filmRepo;
        }

    }
}
