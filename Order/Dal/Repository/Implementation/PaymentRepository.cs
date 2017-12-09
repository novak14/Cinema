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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly OrderOptions _options;
        private ILogger<PaymentRepository> _logger;

        public PaymentRepository(IOptions<OrderOptions> options, ILogger<PaymentRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public Payment Add(int IdMethod, decimal Price)
        {
            var sql = @"insert Payment(IdMethod,Price) values(@IdMethod,@Price);
SELECT * FROM Payment WHERE IdPayment = SCOPE_IDENTITY();";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var dd = connection.Query<Payment>(sql, new { IdMethod = IdMethod, Price = Price }).Single();
                //var dom = connection.Execute(sql, new { IdMethod = IdMethod, Price = Price });
                //var d = connection.Query<Payment>("SELECT * FROM Payment WHERE IdPayment = SCOPE_IDENTITY();").LastOrDefault();
                return dd;
            }
        }
    }
}
