﻿using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models.OrderViewModels
{
    public class PlaceViewModel
    {
        public PlaceViewModel() { }
        public PlaceViewModel(int IdFilm, string Date)
        {
            this.IdFilm = IdFilm;
            this.Date = Date;
        }
        public List<Places> plac { get; set; } 
        public int IdFilm { get; set; }
        public string Date { get; set; }
    }
}
