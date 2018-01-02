using Catalog.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Dal.Repository.Abstraction
{
    public interface IFilmRepository
    {
        /// <summary>
        /// Ziska film podle id
        /// </summary>
        /// <param name="id">Unikatni cislo filmu</param>
        /// <returns></returns>
        Film GetFilm(int id);

        /// <summary>
        /// Ziska veskere informace o urcitem filmu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Film GetOneFilm(int id);

        /// <summary>
        /// Ziska vsechny filmy podle datumu
        /// </summary>
        /// <returns></returns>
        List<December> GetProgramFilms();

        /// <summary>
        /// Data pro pet titulnich filmu
        /// </summary>
        /// <returns></returns>
        List<Film> HomePage();

        /// <summary>
        /// Ziska filmy pro HomePage podle datumu 
        /// </summary>
        /// <returns></returns>
        List<December> GetHomePage();
    }
}
