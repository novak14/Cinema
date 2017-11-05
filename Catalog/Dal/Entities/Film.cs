using System;
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

        public virtual Access Access{ get; set; }
        public virtual Price Price { get; set; }
        public virtual Dabing Dabing { get; set; }
        public virtual Time Time { get; set; }

        public virtual ICollection<Film_dim> Film_dim { get; set; }
        public virtual ICollection<Film_type> Film_type { get; set; }



    }
}
