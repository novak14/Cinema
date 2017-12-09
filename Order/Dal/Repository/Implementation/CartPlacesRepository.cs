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
    public class CartPlacesRepository : ICartPlacesRepository
    {
        private readonly OrderOptions _options;
        private ILogger<CartPlacesRepository> _logger;

        public CartPlacesRepository(IOptions<OrderOptions> options, ILogger<CartPlacesRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public void Add(int IdCartFilm, int IdPlace)
        {
            string sql = @"insert CartPlaces(IdCartFilm,IdPlace) values(@IdCartFilm,@IdPlace);";

            using (var connection = new SqlConnection(_options.connectionString))
            {
                var dom = connection.Execute(sql, new { IdCartFilm = IdCartFilm, IdPlace = IdPlace });
            }
        }
    }
}
