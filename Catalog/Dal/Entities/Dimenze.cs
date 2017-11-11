using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Dimenze
    {
        [Key]
        public int IdDim { get; set; }
        public string DimensionType { get; set; }

        public virtual List<FilmDim> Film_dim { get; set; }

    }
}
