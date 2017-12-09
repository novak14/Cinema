using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Entities
{
    public class OrderFilm
    {
        public int IdOrder { get; set; }
        public int IdFilm { get; set; }
        public int Amount { get; set; }
        public DateTime Time { get; set; }
        public DateTime Date { get; set; }
        public int IdCartFilm { get; set; }
    }
}
