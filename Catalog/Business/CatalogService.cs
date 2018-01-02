using Catalog.Dal.Entities;
using Catalog.Dal.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Business
{
    public class CatalogService
    {
        private IFilmRepository _filmRepo;

        public CatalogService (IFilmRepository filmRepo)
        {
            _filmRepo = filmRepo;
        }

        /// <summary>
        /// Ziska film podle id
        /// </summary>
        /// <param name="id">Unikatni cislo filmu</param>
        /// <returns></returns>
        public Film GetFilm(int id)
        {
            var result = _filmRepo.GetFilm(id);
            return (result);
        }

        /// <summary>
        /// Ziska veskere informace o urcitem filmu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Film GetOneFilm(int id)
        {
            return _filmRepo.GetOneFilm(id);
        }

        /// <summary>
        /// Ziska vsechny filmy podle datumu
        /// </summary>
        /// <returns></returns>
        public List<December> GetDateFilms()
        {
            return _filmRepo.GetProgramFilms();
        }

        /// <summary>
        /// Zobrazi pet titulnich filmu
        /// </summary>
        /// <returns></returns>
        public List<Film> HomePage()
        {
            return _filmRepo.HomePage();
        }

        /// <summary>
        /// Ziska filmy pro HomePage podle datumu 
        /// </summary>
        /// <returns></returns>
        public List<December> GetHomePage()
        {
            return _filmRepo.GetHomePage();
        }
    }
}
