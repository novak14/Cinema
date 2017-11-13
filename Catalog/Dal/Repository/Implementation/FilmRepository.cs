using Catalog.Configuration;
using Catalog.Dal.Context;
using Catalog.Dal.Entities;
using Catalog.Dal.Repository.Abstraction;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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

        //Uvodni obrazovka
        public List<Film> GetSpecificFilms()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            var sql = @"SELECT F.name, D.name, A.Age, P.OverallPrice, T.OverallTime, C.DimensionType FROM Film AS F
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
                ON B.IdDim = C.IdDim;";

            var lookup = new Dictionary<int, Film>();
            List<Film> filmList = new List<Film>();
            using (var connection = new SqlConnection(connectionString))
            {
                var t = connection.Query<Film, Access, Dabing, Price, Time, Dimenze, Film>(sql, (film, access, dabing, price, time, dimenze) =>
                {
                    Film fi;
                    if (!lookup.TryGetValue(film.IdFilm, out fi))
                        lookup.Add(film.IdFilm, fi = film);

                    fi.Dabing = dabing;
                    fi.Access = access;
                    fi.Price = price;
                    fi.Time = time;
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
                }, splitOn: "IdAcc,IdDab, IdPrice, IdTime, IdDim").AsQueryable();

                filmList = lookup.Values.ToList();
            }
            return filmList;
        }

        //Program
        public List<Film> GetProgramFilms()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            var sql = @"SELECT F.name, F.image, P.OverallPrice, T.OverallTime FROM Film AS F
            JOIN Price AS P
                ON F.IdPrice = P.IdPrice
            JOIN Time AS T
                ON F.idTime = T.IdTime;";

            var lookup = new Dictionary<int, Film>();
            List<Film> filmList = new List<Film>();
            using (var connection = new SqlConnection(connectionString))
            {
                var t = connection.Query<Film, Price, Time, Film>(sql, (film, price, time) =>
                {
                    Film fi;
                    if (!lookup.TryGetValue(film.IdFilm, out fi))
                        lookup.Add(film.IdFilm, fi = film);

                    fi.Price = price;
                    fi.Time = time;

                    return film;
                }, splitOn: "IdAcc,IdDab, IdPrice, IdTime ").AsQueryable();

                filmList = lookup.Values.ToList();
            }
            return filmList;
        }


        public List<Film> GetAllFilms()
        {

            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";
            var sql = @"SELECT * FROM Film AS F
            LEFT JOIN DateFilm AS DF
                ON F.IdFilm= DF.IdFilm
            LEFT JOIN December AS DC
                ON DF.IdDate = DC.IdDate
			JOIN Access AS A
				ON F.IdAccess = A.IdAcc
			JOIN Price AS P
				ON F.IdPrice = P.IdPrice
			JOIN Time AS T
				ON F.idTime = T.IdTime
			JOIN Dabing AS D
				ON F.IdDab = D.IdDab
			LEFT JOIN FilmDim AS FD
				ON F.IdFilm = FD.IdFilm
			LEFT JOIN Dimenze AS DZ
				ON FD.IdDim = DZ.IdDim;";

            var lookup = new Dictionary<int, Film>();
            List<Film> filmList = new List<Film>();
            using (var connection = new SqlConnection(connectionString))
            {
                var t = connection.Query<Film, December, Access, Price, Time, Dabing, Dimenze, Film>(sql, (film, december, access, price, time, dabing, dimenze) =>
                {
                    Film fi;
                    if (!lookup.TryGetValue(film.IdFilm, out fi))
                        lookup.Add(film.IdFilm, fi = film);

                    fi.Dabing = dabing;
                    fi.Access = access;
                    fi.Price = price;
                    fi.Time = time;

                    if (fi.December == null)
                    {
                        fi.December = new List<December>();
                    }
                    if (december != null)
                    {
                        fi.December.Add(december);
                        fi.December = fi.December.GroupBy(i => i.IdDate)
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
                }, splitOn: "IdDate, IdAcc, IdPrice, IdTime, IdDab, IdDim").AsQueryable();

                filmList = lookup.Values.ToList();
            }
            return filmList;

            //var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            //var sql = @"SELECT * FROM Film AS F
            //JOIN Access AS A
            //    ON F.IdAccess = A.IdAcc
            //JOIN Dabing AS D
            //    ON F.IdDab = D.IdDab
            //JOIN Price AS P
            //    ON F.IdPrice = P.IdPrice
            //JOIN Time AS T
            //    ON F.idTime = T.IdTime
            //LEFT JOIN FilmDim AS B
            //    ON F.IdFilm = B.IdFilm
            //LEFT JOIN Dimenze AS C
            //    ON B.IdDim = C.IdDim
            //LEFT JOIN DateFilm AS DF
            //    ON F.IdFilm= DF.IdFilm
            //LEFT JOIN December AS DC
            //    ON DF.IdDate = DC.IdDate;";

            //var lookup = new Dictionary<int, Film>();
            //List<Film> filmList = new List<Film>();
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    var t = connection.Query<Film, Access, Dabing, Price, Time, December, Dimenze, Film>(sql, (film, access, dabing, price, time, december, dimenze) =>
            //    {
            //        Film fi;
            //        if (!lookup.TryGetValue(film.IdFilm, out fi))
            //            lookup.Add(film.IdFilm, fi = film);

            //        fi.Dabing = dabing;
            //        fi.Access = access;
            //        fi.Price = price;
            //        fi.Time = time;

            //        if (fi.December == null)
            //        {
            //            fi.December = new List<December>();
            //        }
            //        if (december != null)
            //        {
            //            fi.December.Add(december);
            //            fi.December = fi.December.GroupBy(i => i.IdDate)
            //                .Select(g => g.First()).ToList();
            //        }
            //        if (fi.Dimenze == null)
            //        {
            //            fi.Dimenze = new List<Dimenze>();
            //        }
            //        if (dimenze != null)
            //        {
            //            fi.Dimenze.Add(dimenze);
            //            fi.Dimenze = fi.Dimenze.GroupBy(i => i.IdDim)
            //           .Select(g => g.First()).ToList();
            //        }
            //        return film;
            //    }, splitOn: "IdAcc,IdDab, IdPrice, IdTime, IdDim, IdDate").AsQueryable();

            //    filmList = lookup.Values.ToList();
            //}
            //return filmList;
        }

        //Jednotlivy film
        public List<Film> GetOneFilm()
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

        public List<December> GetDateFilms()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            var sql = @"SELECT * FROM December AS F
            LEFT JOIN DateFilm AS DF
                ON F.IdDate= DF.IdDate
            LEFT JOIN Film AS DC
                ON DF.IdFilm = DC.IdFilm
			JOIN Price AS P
				ON DC.IdPrice = P.IdPrice
			JOIN Time AS T
				ON DC.idTime = T.IdTime;";

            var lookup = new Dictionary<int, December>();
            List<December> filmList = new List<December>();
            using (var connection = new SqlConnection(connectionString))
            {
                var t = connection.Query<December, Film, Price, Time, December>(sql, (december, film, price, time) =>
                {
                    December fi;
                    if (!lookup.TryGetValue(december.IdDate, out fi))
                        lookup.Add(december.IdDate, fi = december);

                    if (fi.Film == null)
                    {
                        fi.Film = new List<Film>();
                    }
                    if (film != null)
                    {
                        film.Price = price;
                        film.Time = time;
                        fi.Film.Add(film);
                        //fi.Film = fi.Film.GroupBy(i => i.IdFilm)
                        //    .Select(g => g.First()).ToList();
                    }

                    return december;
                }, splitOn: "IdFilm, IdPrice, IdTime").AsQueryable();

                filmList = lookup.Values.ToList();
            }
            return filmList;
        }
    }
}
