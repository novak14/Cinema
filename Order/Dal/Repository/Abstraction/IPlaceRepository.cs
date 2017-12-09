using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface IPlaceRepository
    {
        /// <summary>
        /// Pokud pro tento film uz existuji objednavky zajisti zobrazeni pouze volnych mist
        /// </summary>
        /// <param name="plac"></param>
        /// <param name="IdFilm"></param>
        /// <param name="IdDate"></param>
        /// <returns></returns>
        List<Places> CheckFreePlaces(List<Places> plac, int IdFilm, DateTime IdDate);

        /// <summary>
        /// Zobrazi vsechny mozne sedacky
        /// </summary>
        /// <param name="IdFilm"></param>
        /// <param name="IdDate"></param>
        /// <returns></returns>
        List<Places> ShowAllSeats(int IdFilm, DateTime IdDate);
    }
}
