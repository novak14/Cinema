using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Dal.Entities
{
    public class Places
    {
        public int IdPlace { get; set; }
        public int IdNumberPlace { get; set; }
        public int Rows { get; set; }

        public bool checkboxAnswer { get; set; }
    }
}
