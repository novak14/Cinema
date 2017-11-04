using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Film_dim
    {

        public int IdFilm { get; set; }
        public virtual Film Film { get; set; }

        public int IdDim { get; set; }
        public virtual Dimension Dimension { get; set; }
    }
}
