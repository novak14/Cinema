using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class FilmDim
    {
        [Key]
        public int IdFilm { get; set; }
        public Film Film { get; set; }

        [Key]
        public int IdDim { get; set; }
        public Dimenze Dimension { get; set; }
    }
}
