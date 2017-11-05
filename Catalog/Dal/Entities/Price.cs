using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Price
    {
        [Key]
        public int IdPrice { get; set; }
        public decimal OverallPrice { get; set; }

        public ICollection<Film> Film { get; set; }

    }
}
