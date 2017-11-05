using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Time
    {
        [Key]
        public int IdTime { get; set; }
        public DateTime OverallTime { get; set; }

        public ICollection<Film> Film { get; set; }

    }
}
