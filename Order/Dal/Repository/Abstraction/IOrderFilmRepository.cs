using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Repository.Abstraction
{
    public interface IOrderFilmRepository
    {
        /// <summary>
        /// Prida polozku do OrderFilm
        /// </summary>
        /// <param name="IdOrder"></param>
        /// <param name="IdFilm"></param>
        /// <param name="Amount"></param>
        /// <param name="Time"></param>
        /// <param name="Date"></param>
        /// <param name="IdCartFilm"></param>
        void Add(int IdOrder, int IdFilm, int Amount, DateTime Time, DateTime Date, int IdCartFilm);
    }
}
