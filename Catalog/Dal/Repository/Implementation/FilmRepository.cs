using Catalog.Configuration;
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

        public FilmRepository(IOptions<CatalogOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options.Value;
        }

        /// <summary>
        /// Data pro jeden film
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Film GetOneFilm(int id)
        {

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
            using (var connection = new SqlConnection(_options.connectionString))
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

        /// <summary>
        /// Filmy pro programovou stranku
        /// </summary>
        /// <returns></returns>
        public List<December> GetProgramFilms()
        {          
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
            using (var connection = new SqlConnection(_options.connectionString))
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
                     }

                     return december;
                 }, splitOn: "IdFilm,IdDim,IdPrice,IdTime,IdAcc").AsQueryable();

                filmList = lookup.Values.ToList();
            }
            
            return filmList;
        }

        public List<December> GetHomePage()
        {
            var DateOfMonth = DateTime.Now.Date;

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
                ON DC.IdAccess = A.IdAcc
WHERE F.DateOfMonth = @DateOfMonth";

            var lookup = new Dictionary<int, December>();
            List<December> filmList = new List<December>();
            using (var connection = new SqlConnection(_options.connectionString))
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
                    }

                    return december;
                }, new { DateOfMonth = DateOfMonth }, splitOn: "IdFilm,IdDim,IdPrice,IdTime,IdAcc").AsQueryable();

                filmList = lookup.Values.ToList();
            }

            return filmList;
        }

        public Film GetFilm(int idFilm)
        {

            var sql = @"SELECT * FROM Film
                JOIN Time ON Time.IdTime = Film.idTime                  
                WHERE FILM.IdFilm = @id;";
            var lookup = new Dictionary<int, Film>();
            Film filmOne = new Film();
            using (var connection = new SqlConnection(_options.connectionString))
            {
                filmOne = connection.Query<Film, Time, Film>(sql, (film, time) =>
                {
                    film.Time = time;
                    return film;
                }, new { id = idFilm }, splitOn: "IdTime").FirstOrDefault();
            }
            return filmOne;

        }

        public List<Film> HomePage()
        {
            var sql = @"Select Top(5) IdFilm, image From Film;";

            using (var connection = new SqlConnection(_options.connectionString))
            {
                var filmOne = connection.Query<Film>(sql).ToList();
                return filmOne;
            }
        }
    }
}
