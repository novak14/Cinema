using Catalog.Dal.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Order.Dal.Entities
{
    public class CartFilm
    {
        [Key]
        public int IdCartFilm { get; set; }
        public string IdUser { get; set; }
        public int IdFilm { get; set; }
        public int? Amount { get; set; }
        public DateTime IdTime { get; set; }
        public DateTime IdDate { get; set; }

        public Film Film { get; set; }
        public List<Places> CartPlaces { get; set; } = new List<Entities.Places>();
    }
}
