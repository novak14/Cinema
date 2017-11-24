using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class December
    {
        [Key]
        public int IdDate { get; set; }
        public DateTime DateOfMonth { get; set; }

        public virtual List<DateFilm> DateFilm { get; set; } = new List<DateFilm>();

        public List<Film> Film { get; set; }

        public Dimenze Dimenze { get; set; }

    }
}
    