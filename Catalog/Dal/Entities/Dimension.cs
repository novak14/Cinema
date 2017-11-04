using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Dimension
    {
        [Key]
        public int IdDim { get; set; }
        public string DimensionType { get; set; }

        public virtual List<Film_dim> Film_dim { get; set; }

    }
}
