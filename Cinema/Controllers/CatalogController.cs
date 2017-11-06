using Catalog.Business;
using Catalog.Dal.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using Dapper;

namespace Cinema.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly CatalogDbContext _context;
        private readonly ILogger _logger;

        //private IDbConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);



        public CatalogController(CatalogDbContext context, CatalogService catalogService, ILoggerFactory loggerFactory)
        {
            _context = context;
            _catalogService = catalogService;
            _logger = loggerFactory.CreateLogger<CatalogController>();
        }


        public List<Film> ReadAll()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            //var connectionString = ConfigurationManager.ConnectionStrings["WingtipToys"].ConnectionString;


            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var pom = db.Query<Film>
                    ("Select * From Film").ToList();
                return pom;
            }
        }

        public IActionResult Index()
        {
            // Dapper
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var pom = db.Query<Film>
                    ("Select * From Film").ToList();
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                int pom = 0;
                var invoiceDictionary = new Dictionary<int, Film>();
                string sql = "SELECT * FROM Film AS A JOIN Access AS B ON A.IdAccess= B.IdAcc;";

                var invoices = db.Query<Film, Access, Film>(
                        sql,
                        (film, access) =>
                        {
                            Film invoiceEntry;

                            if (!invoiceDictionary.TryGetValue(film.IdAccess, out invoiceEntry))
                            {
                                invoiceEntry = film;
                                invoiceEntry.Access = new List<Access>();
                                invoiceDictionary.Add(invoiceEntry.IdAccess, invoiceEntry);
                                pom++;
                            }

                            invoiceEntry.Access.Add(access);
                            return invoiceEntry;
                        },
                        splitOn: "IdAcc")
                        .Distinct()
                    .ToList();
            }

            var courses = _context.Film.Include(c => c.Dabing).ToList();
            //var count = courses.Count();
            var manyToMany = _context.Film_dim.Include(c => c.Dimension).ToList();

            var filmType = _context.Film_type.Include(c => c.Type).ToList();
            //var countMany = manyToMany.Count();

            var many = _context.Film.Include(c => c.Access).Include(c => c.Dabing).Include(c => c.Price).Include(c => c.Time).Include(c => c.Film_dim).Include(c => c.Film_type).ToList();
            
            foreach (var item in many)
            {
                //var dim = item.Film_dim.ToList()[0].Dimension.DimensionType;
                //var tmp = item.Film_type.ToList()[0].Type.Genre;
                var pom = item.Film_dim.ToList();
            }

            //foreach (var item in filmType)
            //{
            //    var pom = item.Film.Name;
            //    var pom1 = item.Type.Genre;
            //    var pom2 = item.Film.Film_dim;
            //}

            //foreach (var item in courses)
            //{
            //    var pom = item.Access.Age;
            //}

            

            //foreach (var item in manyToMany)
            //{
            //    var pom = item.Film.Access.Age;
            //    var pom1 = item.Film.Dabing.Name;
            //    var pom2 = item.Film.Price.OverallPrice;
            //    var pom3 = item.Film.Time.OverallTime;
            //    foreach (var tmp in item.Film.Film_type)
            //    {
            //        var pom4 = tmp.Type.Genre;
            //    }
            //    var kkk = item.Dimension.DimensionType;
            //}

            return View(many);
        }

    }
}
