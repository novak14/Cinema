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

        public NewOrder Add(string IdUser, int IdPayment, int IdDeliveryType, DateTime date)
        {
            var sql = @"insert NewOrder(IdUser,IdPayment,IdDeliveryType,CreateDate) values(@IdUser,@IdPayment,@IdDeliveryType,@CreateDate);";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var dom = connection.Execute(sql, new { IdUser = IdUser, IdPayment = IdPayment, IdDeliveryType = IdDeliveryType, CreateDate = date });
                var d = connection.Query<NewOrder>("Select IdOrder From NewOrder Where IdUser = @IdUser", new { IdUser = IdUser }).LastOrDefault();
                return d;
            }
        }

        public List<NewOrder> GetAllOrders(string IdUser)
        {
            var sql = @"SELECT * FROM NewOrder
LEFT JOIN Payment ON PAYMENT.IdPayment = NewOrder.IdPayment
 WHERE NewOrder.IdUser = @IdUser";
            using (var connection = new SqlConnection(_options.connectionString))
            {
                var d = connection.Query<NewOrder, Payment, NewOrder>(sql, (newOrder, payment) => 
                {
                    newOrder.Payment = payment;
                    return newOrder;
                }, new { IdUser = IdUser }, splitOn: "IdPayment").ToList();
                return d;
            }
        }

        public List<NewOrder> GetHistoryOrder(int IdOrder)
        {
            var sql = @"SELECT 
	NewOrder.IdOrder,
	NewOrder.CreateDate,
	OrderFilm.Time,
	OrderFilm.Date,
	OrderFilm.Amount,
	PaymentMethod.MethodType,
	DeliveryType.TypeDelivery,
	Film.name,
	Film.image,
    Film.IdFilm,
	Places.IdNumberPlace,
    Places.Rows,
    Price.OverallPrice
FROM NewOrder
LEFT JOIN OrderFilm ON OrderFilm.IdOrder = NewOrder.IdOrder
LEFT JOIN Payment ON Payment.IdPayment = NewOrder.IdPayment
LEFT JOIN PaymentMethod ON PaymentMethod.IdMethod = Payment.IdMethod
LEFT JOIN DeliveryType ON DeliveryType.IdDeliveryType = NewOrder.IdDeliveryType
LEFT JOIN Film ON Film.IdFilm = OrderFilm.IdFilm
LEFT JOIN Price ON Price.IdPrice = Film.IdPrice
LEFT JOIN CartPlaces ON CartPlaces.IdCartFilm = OrderFilm.IdCartFilm
LEFT JOIN Places ON Places.IdPlace = CartPlaces.IdPlace
WHERE NewOrder.IdOrder = @IdOrder";

            var lookup = new Dictionary<int, NewOrder>();

            using (var connection = new SqlConnection(_options.connectionString))
            {
                var d = connection.Query<NewOrder, OrderFilm, PaymentMethod, DeliveryType, Film, Places, Price, NewOrder>(sql, (newOrder, orderFilm, paymentMethod, deliveryType, film, cartPlaces, price) =>
                {
                    NewOrder FilmDic;

                    newOrder.Film = film;

                    if (!lookup.TryGetValue(newOrder.Film.IdFilm, out FilmDic))
                        lookup.Add(newOrder.Film.IdFilm, FilmDic = newOrder);

                    FilmDic.PaymentMethod = paymentMethod;
                    FilmDic.DeliveryType = deliveryType;
                    FilmDic.OrderFilm = orderFilm;
                    film.Price = price;
                    FilmDic.Film = film;
                    FilmDic.Places.Add(cartPlaces);
                    return FilmDic;
                }, new { IdOrder = IdOrder }, splitOn: "Time,MethodType,TypeDelivery,name,IdNumberPlace,OverallPrice").ToList();

                var look = lookup.Values.ToList();
                return look;
            }
        }
    }
}
