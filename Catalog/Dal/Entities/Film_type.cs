using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Film_type
    {
        public int IdType { get; set; }
        public virtual Type Type { get; set; }

        public int IdFilm { get; set; }
        public virtual Film Film { get; set; }
    }
}
