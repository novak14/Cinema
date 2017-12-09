using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Configuration;
using Order.Dal.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Order.Dal.Repository.Implementation
{
    public class OrderFilmRepository : IOrderFilmRepository
    {
        private readonly OrderOptions _options;
        private ILogger<OrderFilmRepository> _logger;

        public OrderFilmRepository(IOptions<OrderOptions> options, ILogger<OrderFilmRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public void Add(int IdOrder, int IdFilm, int Amount, DateTime Time, DateTime Date, int IdCartFilm)
        {
            var sql = @"insert OrderFilm(IdOrder,IdFilm,Amount,Time,Date,IdCartFilm) values(@IdOrder,@IdFilm,@Amount,@Time,@Date,@IdCartFilm);";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var dom = connection.Execute(sql, new { IdOrder = IdOrder, IdFilm = IdFilm, Amount = Amount, Time = Time, Date = Date, IdCartFilm = IdCartFilm});
            }
        }
    }
}
