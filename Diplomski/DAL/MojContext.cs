using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace Diplomski.DAL
{
    public class MojContext:DbContext
    {
        public MojContext()
                 : base("Name=MojConnectionString")
        {

        }

        public DbSet<VrstaAktivnosti> VrsteAktivnosti { get; set; }
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Uloga> Uloge { get; set; }
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Semestar> Semestri { get; set; }
        public DbSet<Aktivnost> Aktivnosti { get; set; }
        public DbSet<AktivnostStudent> AktivnostStudent { get; set; }
        public DbSet<SlusaPredmet> SlusaPredmet { get; set; }
        public DbSet<NastavnoOsoblje> NastavnoOsoblje { get; set; }
        public DbSet<PredajePredmet> PredajePredmet { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Korisnik>().HasOptional(x => x.Student).WithRequired(x => x.Korisnik);
            modelBuilder.Entity<Korisnik>().HasOptional(x => x.NastavnoOsoblje).WithRequired(x => x.Korisnik);


            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<SlusaPredmet>().HasOptional(x => x.Student);
            // modelBuilder.Entity<Osoba>().HasOptional(x => x.Korisnik).WithRequired(x => x.Osoba);
        }
    }
}