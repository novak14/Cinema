using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface ICartFilmRepository
    {
        /// <summary>
        /// Prida film do kosiku
        /// </summary>
        /// <param name="IdUser"></param>
        /// <param name="IdFilm"></param>
        /// <param name="IdTime"></param>
        /// <param name="IdDate"></param>
        void Add(string IdUser, int IdFilm, DateTime IdTime, DateTime IdDate);

        /// <summary>
        /// Prida mnozstvi koupenych mist
        /// </summary>
        /// <param name="IdCartFilm"></param>
        /// <param name="Amount"></param>
        void Update(int IdCartFilm, int Amount);

        /// <summary>
        /// Vrati posledni kosik, ktery byl vytvoren pro uzivatele
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        CartFilm GetLastCart(string IdUser);

        /// <summary>
        /// Ziska vsechny polozky uzivatele
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        List<CartFilm> GetUserCart(string IdUser);

        /// <summary>
        /// Ziska polozky pro zobrazeni v Summary
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        List<CartFilm> GetSummary(string IdUser);


        /// <summary>
        /// Testuje jestli uz si nekdo vybral pro tento film na tento den nejaka mista
        /// </summary>
        /// <param name="IdDate"></param>
        /// <param name="IdFilm"></param>
        /// <returns></returns>
        int TestIfFirst(DateTime IdDate, int IdFilm);

        /// <summary>
        /// Vymaze danou polozku
        /// </summary>
        /// <param name="IdCartFilm"></param>
        void DeleteItem(int IdCartFilm);

    }
}
