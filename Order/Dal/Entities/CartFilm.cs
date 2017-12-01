using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Entities
{
    public class CartFilm
    {
        public int IdCartFilm { get; set; }
        public string IdUser { get; set; }
        public int IdFilm { get; set; }
        public int Amount { get; set; }
        public int IdTime { get; set; }
        public int IdDate { get; set; }
    }
}
