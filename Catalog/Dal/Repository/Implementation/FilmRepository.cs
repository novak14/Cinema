using Catalog.Configuration;
using Catalog.Dal.Context;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Dal.Repository.Abstraction;
using Catalog.Dal.Entities;
using System.Linq;
using System.Data.SqlClient;
using Dapper;

namespace Catalog.Dal.Repository.Implementation
{
    public class FilmRepository : IFilmRepository
    {
        private readonly CatalogOptions _options;
        private ILogger<FilmRepository> _logger;
        private readonly CatalogDbContext _context;

        public FilmRepository(IOptions<CatalogOptions> options, ILogger<FilmRepository> logger, CatalogDbContext catalogDbContext)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
            _logger = logger;
            _context = catalogDbContext;
        }

        public Film GetFilm(int id)
        {
            var result = _context.Film.FirstOrDefault(s => (s.IdDab == id));
            return result;
        }

        public List<Film> GetAllFilms()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            var sql = @"SELECT * FROM Film AS F
            JOIN Access AS A
                ON F.IdAccess = A.IdAcc
            JOIN Dabing AS D
                ON F.IdDab = D.IdDab
            JOIN Price AS P
                ON F.IdPrice = P.IdPrice
            JOIN Time AS T
                ON F.idTime = T.IdTime
            LEFT JOIN FilmDim AS B
                ON F.IdFilm = B.IdFilm
            LEFT JOIN Dimenze AS C
                ON B.IdDim = C.IdDim
            LEFT JOIN Film_type AS L
                ON F.IdFilm = L.IdFilm
            LEFT JOIN Type AS K
                ON L.IdType = K.IdType;";

            var lookup = new Dictionary<int, Film>();
            List<Film> filmList = new List<Film>();
            using (var connection = new SqlConnection(connectionString))
            {
                var t = connection.Query<Film, Access, Dabing, Price, Time, Dimenze, Entities.Type, Film>(sql, (film, access, dabing, price, time, dimenze, type) =>
                {
                    Film fi;
                    if (!lookup.TryGetValue(film.IdFilm, out fi))
                        lookup.Add(film.IdFilm, fi = film);

                    fi.Dabing = dabing;
                    fi.Access = access;
                    fi.Price = price;
                    fi.Time = time;
                    if (fi.Type == null)
                    {
                        fi.Type = new List<Entities.Type>();
                    }
                    if (type != null)
                    {
                        fi.Type.Add(type);
                        fi.Type = fi.Type.GroupBy(i => i.IdType)
                       .Select(g => g.First()).ToList();
                    }
                    if (fi.Dimenze == null)
                    {
                        fi.Dimenze = new List<Dimenze>();
                    }
                    if (dimenze != null)
                    {
                        fi.Dimenze.Add(dimenze);
                        fi.Dimenze = fi.Dimenze.GroupBy(i => i.IdDim)
                       .Select(g => g.First()).ToList();
                    }
                    return film;
                }, splitOn: "IdAcc,IdDab, IdPrice, IdTime, IdDim, IdType").AsQueryable();

                filmList = lookup.Values.ToList();
            }
            return filmList;
        }
    }
}
