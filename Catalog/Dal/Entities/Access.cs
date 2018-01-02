using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Access
    {
        [Key]
        public int IdAcc { get; set; }
        public int Age { get; set; }

        public List<Film> Film { get; set; }
    }
}
