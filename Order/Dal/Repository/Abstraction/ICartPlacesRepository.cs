using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface ICartPlacesRepository
    {
        /// <summary>
        /// Prida polozku do tabulky CartPlaces
        /// </summary>
        /// <param name="IdCartFilm"></param>
        /// <param name="IdPlace"></param>
        void Add(int IdCartFilm, int IdPlace);
    }
}
