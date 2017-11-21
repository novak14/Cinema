using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class DateFilm
    {
        public int IdDate { get; set; }
        public December December { get; set; }

        public int IdFilm { get; set; }
        public Film Film { get; set; }
    }
}
