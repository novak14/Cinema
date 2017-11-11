using Catalog.Configuration;
using Catalog.Dal.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;




namespace Catalog.Dal.Context
{
    public class CatalogDbContext : DbContext
    {

        /***************************************************************/
        /*      NASTAVENI DB - KONFIGURACE, OPTIONS */
        /***************************************************************/

        private readonly CatalogOptions _options2;

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IOptions<CatalogOptions> options2) : base(options)
        {
            if (options2 == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options2 = options2.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_options2.connectionString);
        }


        /***************************************************************/
        /*      ENTITY */
        /***************************************************************/
        public DbSet<Film> Film { get; set; }
        public DbSet<Access> Access { get; set; }
        public DbSet<Dabing> Dabing{ get; set; }
        public DbSet<Days> Days { get; set; }
        public DbSet<Dimenze> Dimenze{ get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Time> Time { get; set; }
        public DbSet<Entities.Type> Type { get; set; }
        public DbSet<FilmDim> FilmDim { get; set; }
        public DbSet<Film_type> Film_type { get; set; }
        






        /***************************************************************/
        /*      VAZBY ENTIT */
        /***************************************************************/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Dimenze>()
            //    .HasMany(x => x.Film)
            //    .WithMany(x => x.)
            //    .Map(x =>
            //    {
            //        x.ToTable("FilmDim"); // third table is named Cookbooks
            //        x.MapLeftKey("IdFilm");
            //        x.MapRightKey("IdDim");
            //    });

            //modelBuilder.Entity<Film_type>()
            //    .HasKey(t => new { t.IdFilm, t.IdType });

            //modelBuilder.Entity<FilmDim>()
            //    .HasKey(t => new { t.IdFilm, t.IdDim });

            //modelBuilder.Entity<FilmDim>()
            //    .HasOne(pt => pt.Film)
            //    .WithMany(p => p.FilmDim)
            //    .HasForeignKey(pt => pt.IdFilm);

            //modelBuilder.Entity<FilmDim>()
            //    .HasOne(pt => pt.Dimension)
            //    .WithMany(t => t.Film_dim)
            //    .HasForeignKey(pt => pt.IdDim);

            //modelBuilder.Entity<Film>().HasKey(c => c.IdFilm);

            base.OnModelCreating(modelBuilder);
        }




        /***************************************************************/
        /*    OVERRIDE  METHODS */
        /***************************************************************/
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }




    }
}
