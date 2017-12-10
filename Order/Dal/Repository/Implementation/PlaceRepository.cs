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
    public class PlaceRepository : IPlaceRepository
    {
        private readonly OrderOptions _options;
        private ILogger<PlaceRepository> _logger;

        public PlaceRepository(IOptions<OrderOptions> options, ILogger<PlaceRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public List<Places> CheckFreePlaces(List<Places> plac, int IdFilm, DateTime IdDate)
        {
            string sql3 = @"SELECT DISTINCT Places.IdPlace, Places.IdNumberPlace, OrderFilm.Date
  FROM [Cinema].[dbo].[Places]
  LEFT JOIN [Cinema].[dbo].CartPlaces ON Places.IdPlace = CartPlaces.IdPlace
  LEFT JOIN [Cinema].[dbo].OrderFilm ON OrderFilm.IdCartFilm = CartPlaces.IdCartFilm AND (OrderFilm.Date = @Date AND OrderFilm.IdFilm = @IdFilm)
  ORDER BY Places.IdPlace, OrderFilm.Date;";

            using (var connection = new SqlConnection(_options.connectionString))
            {
                connection.Query<Places, OrderFilm, Places>(sql3, (place, cartFilm) =>
                {

                    plac.Add(place);
                    if (cartFilm != null)
                    {
                        plac.RemoveAll(c => c.IdPlace == place.IdPlace);
                    }

                    return place;
                }, new { Date = IdDate, IdFilm = IdFilm}, splitOn: "Date");

                return plac;
            }
        }

        public List<Places> ShowAllSeats(int IdFilm, DateTime IdDate)
        {
            string sql2 = @"SELECT DISTINCT Places.IdPlace, Places.IdNumberPlace
  FROM [Cinema].[dbo].[Places]
  LEFT JOIN [Cinema].[dbo].CartPlaces ON Places.IdPlace = CartPlaces.IdPlace
  LEFT JOIN [Cinema].[dbo].OrderFilm ON OrderFilm.IdCartFilm = CartPlaces.IdCartFilm AND (OrderFilm.Date = @Date AND OrderFilm.IdFilm = @IdFilm);";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                return connection.Query<Places>(sql2, new { Date = IdDate, IdFilm = IdFilm }).ToList();
            }
        }
    }
}
