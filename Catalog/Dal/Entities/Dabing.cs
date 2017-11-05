using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Dabing
    {
        [Key]
        public int IdDab { get; set; }
        public string Name { get; set; }

        public ICollection<Film> Film { get; set; }

    }
}
