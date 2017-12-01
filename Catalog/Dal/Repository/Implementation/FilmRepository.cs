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
using System.Linq.Expressions;

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

        //public Film GetFilm(int id)
        //{
        //    var result = _context.Film.FirstOrDefault(s => (s.IdDab == id));
        //    return result;
        //}

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
        }

        //Jednotlivy film
        public Film GetOneFilm(int id)
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
            LEFT JOIN Film_type AS L
                ON F.IdFilm = L.IdFilm
            LEFT JOIN Type AS K
                ON L.IdType = K.IdType
            WHERE F.IdFilm = @id;";

            var sqlMonth = @"SELECT * FROM Film AS F
	JOIN DateFilm AS DF
		ON F.IdFilm = DF.IdFilm
	JOIN December AS DC
		ON DF.IdDate = DC.IdDate
    LEFT JOIN Dimenze AS D
        ON DF.IdDim= D.IdDim
	WHERE f.IdFilm = @id;";




            var lookup = new Dictionary<int, Film>();
            Film filmList = new Film();
            using (var connection = new SqlConnection(connectionString))
            {
                var t = connection.Query<Film, Access, Dabing, Price, Time, Entities.Type, Film>(sql, (film, access, dabing, price, time, type) =>
                {
                    Film fi;

                    if (!lookup.TryGetValue(film.IdFilm, out fi))
                        lookup.Add(film.IdFilm, fi = film);

                    var filmMont = connection.Query<Film, December, Dimenze, Film >(sqlMonth, (filmMonth, december, dimenze) =>
                    {
                        if (fi.December == null)
                        {
                            fi.December = new List<December>();
                        }
                        if (december != null)
                        {
                            december.Dimenze = dimenze;

                            fi.December.Add(december);
                            fi.December = fi.December.GroupBy(i => i.IdDate)
                                .Select(g => g.First()).ToList();
                        }
                        return filmMonth;
                    }, new { id = id }, splitOn: "IdDate,IdDim");

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
                   
                    return film;
                }, new { id = id }, splitOn: "IdAcc,IdDab, IdPrice, IdTime, IdType").AsQueryable();

                filmList = lookup.Values.FirstOrDefault();
            }
            return filmList;
        }

        //public void DapperTests()
        //{
        //    var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";


        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        using (var multi = connection.ExecuteReader(sql))
        //        {
        //            Film pom = new Film();
        //            while (multi.Read())
        //            {
                        
        //            }
        //        }

        //    }
        //}



        public List<December> GetProgramFilms()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            var sql2 = @"Select * From December;";
            


            var sql = @"SELECT * FROM December AS F
            LEFT JOIN DateFilm AS DF
                ON F.IdDate= DF.IdDate
            LEFT JOIN Film AS DC
                ON DF.IdFilm = DC.IdFilm
            LEFT JOIN Dimenze AS D
                ON DF.IdDim= D.IdDim
			JOIN Price AS P
				ON DC.IdPrice = P.IdPrice
			JOIN Time AS T
				ON DC.idTime = T.IdTime
            JOIN Access AS A
                ON DC.IdAccess = A.IdAcc";

            var lookup = new Dictionary<int, December>();
            List<December> filmList = new List<December>();
            using (var connection = new SqlConnection(connectionString))
            {
                    var t = connection.Query<December, Film, Dimenze, Price, Time, Access, December>(sql, (december, film, dimenze, price, time, access) =>
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
                         film.Access = access;
                         film.Dimenzes = dimenze;
                         fi.Film.Add(film);
                         //fi.Film = fi.Film.GroupBy(i => i.IdFilm)
                         //    .Select(g => g.First()).ToList();
                     }

                     return december;
                 }, splitOn: "IdFilm,IdDim,IdPrice,IdTime,IdAcc").AsQueryable();

                filmList = lookup.Values.Take(10).ToList();


                using (var multi = connection.ExecuteReader(sql2))
                {
                    {
                        List<int> pom = new List<int>();
                        while (multi.Read())
                        {
                            pom.Add(1);
                        }
                    }
                }
            }
            
            return filmList;
        }

        public Film GetFilm(int idFilm)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true";

            var sql = @"SELECT * FROM Film
                JOIN Time ON Time.IdTime = Film.idTime                  
                WHERE FILM.IdFilm = @id;";
            var lookup = new Dictionary<int, Film>();
            Film filmOne = new Film();
            using (var connection = new SqlConnection(connectionString))
            {
                filmOne = connection.Query<Film, Time, Film>(sql, (film, time) =>
                {
                    film.Time = time;
                    return film;
                }, new { id = idFilm }, splitOn: "IdTime").FirstOrDefault();
            }
            return filmOne;

        }
    }
}
