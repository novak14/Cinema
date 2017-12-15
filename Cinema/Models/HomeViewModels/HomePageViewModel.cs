using Catalog.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models.HomeViewModels
{
    public class HomePageViewModel
    {
        public HomePageViewModel(List<Film> Film, List<December> December)
        {
            this.Film = Film;
            this.December = December;
        }
        public List<Film> Film { get; set; }
        public List<December> December { get; set; }

    }
}
