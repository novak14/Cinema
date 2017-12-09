using System;
using Microsoft.Extensions.DependencyInjection;
using Order.Business;
using Order.Configuration;
using Order.Dal.Repository.Abstraction;
using Order.Dal.Repository.Implementation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddModuleOrder(this IServiceCollection services, Action<OrderOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }



            //string connectionString = @"Server=DESKTOP-LCV6O88\SQLEXPRESS;Database=AlzaLegoDatabase;User Id=sa;Password=master";
            //services.AddDbContext<EFLocalizationDbContext>(options => options.UseSqlServer(connectionString));




            //registruje nastaveni modulu
            services.Configure(setupAction);

            //connectionString si vezme sam DbContext z IOptions<>

            //REPOSITORY
            services.AddScoped<ICartFilmRepository, CartFilmRepository>();
            services.AddScoped<ICartPlacesRepository, CartPlacesRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
            services.AddScoped<INewOrderRepository, NewOrderRepository>();
            services.AddScoped<IOrderFilmRepository, OrderFilmRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();




            //SERVICES - zapouzdreni vsechn repositories pod jeden objekt
            //Tyto services pak budou pouzivat ostatni tridy/objetky
            services.AddScoped<OrderService, OrderService>();

            return services;
        }
    }
}
