using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Configuration;
using Order.Dal.Entities;
using Order.Dal.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Order.Dal.Repository.Implementation
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly OrderOptions _options;
        private ILogger<DeliveryTypeRepository> _logger;

        public PaymentMethodRepository(IOptions<OrderOptions> options, ILogger<DeliveryTypeRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public List<PaymentMethod> GetAllMethod()
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var sel = connection.Query<PaymentMethod>("Select * From PaymentMethod").ToList();
                return sel;
            }
        }
    }
}
