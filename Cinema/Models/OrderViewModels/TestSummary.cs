using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models.OrderViewModels
{
    public class TestSummary
    {
        public TestSummary()
        {
            Summ = new List<Summary>();
        }
        public List<Summary> Summ { get; set; }

        public decimal OverallPrice { get; set; } = 0;

    }
}

