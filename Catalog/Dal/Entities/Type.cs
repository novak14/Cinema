using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Type
    {
        [Key]
        public int IdType { get; set; }
        public string Genre { get; set; }

        public virtual List<Film_type> Film_type { get; set; }

    }
}
