using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Days
    {
        [Key]
        public int IdDate { get; set; }
        public string Day { get; set; }
    }
}
