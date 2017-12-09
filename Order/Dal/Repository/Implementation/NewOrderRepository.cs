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
    public class NewOrderRepository : INewOrderRepository
    {
        private readonly OrderOptions _options;
        private ILogger<NewOrderRepository> _logger;

        public NewOrderRepository(IOptions<OrderOptions> options, ILogger<NewOrderRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public NewOrder Add(string IdUser, int IdPayment, int IdDeliveryType)
        {
            var sql = @"insert NewOrder(IdUser,IdPayment,IdDeliveryType) values(@IdUser,@IdPayment,@IdDeliveryType);";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var dom = connection.Execute(sql, new { IdUser = IdUser, IdPayment = IdPayment, IdDeliveryType = IdDeliveryType });
                var d = connection.Query<NewOrder>("Select IdOrder From NewOrder Where IdUser = @IdUser", new { IdUser = IdUser }).LastOrDefault();
                return d;
            }
        }
    }
}
