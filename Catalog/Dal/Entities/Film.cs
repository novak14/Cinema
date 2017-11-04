using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Film
    {
        [Key]
        public int id_film { get; set; }
        public string name { get; set; }
        public DateTime length { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public DateTime date_released { get; set; }

        public int id_access { get; set; }
        public int id_price { get; set; }
        public int id_dab { get; set; }

    }
}
