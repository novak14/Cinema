using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class DateFilm
    {
        public int IdDate { get; set; }
        public List<December> December { get; set; }

        public int IdFilm { get; set; }
        public List<Film> Film { get; set; }
    }
}
