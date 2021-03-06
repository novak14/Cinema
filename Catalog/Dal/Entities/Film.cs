﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catalog.Dal.Entities
{
    public class Film
    {
        [Key]
        public int IdFilm { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string backgroundImage { get; set; }
        public string Trailer { get; set; }
        public DateTime DateReleased { get; set; }

        [ForeignKey("Access")]
        public int IdAccess { get; set; }
        [ForeignKey("Price")]
        public int IdPrice { get; set; }
        [ForeignKey("Dabing")]
        public int IdDab { get; set; }
        [ForeignKey("Time")]
        public int IdTime { get; set; }

        public Access Access { get; set; }
        public Price Price { get; set; }
        public Dabing Dabing { get; set; }
        public Time Time { get; set; }
        public Dimenze Dimenzes { get; set; }

        public List<Type> Type { get; set; }
        public List<Dimenze> Dimenze { get; set; } = new List<Dimenze>();
        public List<December> December { get; set; }
        public List<DateFilm> DateFilm { get; set; } = new List<DateFilm>();



    }
}
