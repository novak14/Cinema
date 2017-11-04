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
        public int IdPrice { get; set; }
        public int IdDab { get; set; }
        public int IdTime { get; set; }

        public virtual Access Access{ get; set; }
        public virtual List<Film_dim> Film_dim { get; set; }



    }
}
