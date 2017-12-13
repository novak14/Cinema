using Catalog.Dal.Entities;
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

namespace Order.Dal.Repository.Implementation
{
    public class CartFilmRepository : ICartFilmRepository
    {
        private readonly OrderOptions _options;
        private ILogger<CartFilmRepository> _logger;

        public CartFilmRepository(IOptions<OrderOptions> options, ILogger<CartFilmRepository> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
        }

        public void Add(string IdUser, int IdFilm, DateTime IdTime, DateTime IdDate)
        {
            var sql = @"insert CartFilm(IdUser,IdFilm,IdTime,IdDate) values(@IdUser,@IdFilm,@IdTime,@IdDate);";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var dom = connection.Execute(sql, new { IdUser = IdUser, IdFilm = IdFilm, IdTime = IdTime, IdDate = IdDate });
            }
        }

        public void Update(int IdCartFilm, int Amount)
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                connection.Execute("Update CartFilm Set Amount = @Amount Where IdCartFilm = @IdCartFilm", new { Amount = Amount, IdCartFilm = IdCartFilm });
            }
        }

        public CartFilm GetLastCart(string IdUser, int IdFilm, DateTime IdDate)
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var sel = connection.Query<CartFilm>("Select * From CartFilm Where IdUser = @IdUser AND IdFilm = @IdFilm AND IdDate = @IdDate", new { IdUser = IdUser, IdFilm = IdFilm, IdDate = IdDate }).FirstOrDefault();
                return sel;
            }
        }

        public List<CartFilm> GetSummary(string IdUser)
        {
            string sql = @"Select CartFilm.IdCartFilm, CartFilm.IdFilm, CartFilm.Amount, CartFilm.IdDate, CartFilm.IdTime, Film.name, Film.image, Film.IdDab, Film.IdAccess, Places.IdNumberPlace, Places.Rows, Price.OverallPrice
  From CartFilm
LEFT JOIN Film ON Film.IdFilm = CartFilm.IdFilm
LEFT JOIN CartPlaces ON CartPlaces.IdCartFilm = CartFilm.IdCartFilm
LEFT JOIN Places ON Places.IdPlace = CartPlaces.IdPlace
LEFT JOIN Price ON Price.IdPrice = Film.IdPrice
 Where IdUser = @IdUser AND AMOUNT IS NOT NULL";

            var lookup = new Dictionary<int, CartFilm>();

            using (var connection = new SqlConnection(_options.connectionString))
            {
                var sel = connection.Query<CartFilm, Film, Places, Price, CartFilm>(sql, 
                    (cartFilm, film, cartPlaces, price) =>
                    {
                        CartFilm CF;

                        if (!lookup.TryGetValue(cartFilm.IdCartFilm, out CF))
                            lookup.Add(cartFilm.IdCartFilm, CF = cartFilm);

                        film.Price = price;
                        CF.Film = film;
                        CF.CartPlaces.Add(cartPlaces);
 
                        return CF;
                    },
                    new { IdUser = IdUser }, splitOn: "name,IdNumberPlace,OverallPrice").ToList();
                var t = lookup.Values.ToList();
                return t;
            }
        }

        public int TestIfFirst(DateTime IdDate, int IdFilm)
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                return connection.Query<CartFilm>("SELECT * FROM [CINEMA].DBO.OrderFilm WHERE Date = @Date AND IdFilm = @IdFilm", new { Date = IdDate, IdFilm = IdFilm }).Count();
            }

        }

        public List<CartFilm> GetUserCart(string IdUser)
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var sel = connection.Query<CartFilm, Film, CartFilm>("Select * From CartFilm LEFT JOIN Film ON Film.IdFilm = CartFilm.IdFilm Where IdUser = @IdUser AND AMOUNT IS NOT NULL", (cartFilm, film) => {
                    cartFilm.Film = film;
                    return cartFilm;
                },
                    new { IdUser = IdUser }, splitOn: "IdFilm").ToList();
                return sel;
            }
        }

        public List<CartFilm> GetUserCartForShow(string IdUser)
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var sel = connection.Query<CartFilm, Film, CartFilm>("Select * From CartFilm LEFT JOIN Film ON Film.IdFilm = CartFilm.IdFilm Where IdUser = @IdUser", (cartFilm, film) => {
                    cartFilm.Film = film;
                    return cartFilm;
                },
                    new { IdUser = IdUser }, splitOn: "IdFilm").ToList();
                return sel;
            }
        }

        public void DeleteItem(int IdCartFilm)
        {
            using (var connection = new SqlConnection(_options.connectionString))
            {
                connection.Execute("Delete From CartFilm Where IdCartFilm = @IdCartFilm", new { IdCartFilm = IdCartFilm });
            }
        }
    }
}
