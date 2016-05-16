using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;
using BL.Domain;

namespace DAL
{
        internal class BegrotingDBContext : DbContext 
        {
            public BegrotingDBContext() : base("BegrotingDB_Azure")
            {
            }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<HoofdGemeente>()
                .Property(t => t.naam)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                new IndexAttribute("IX_Naam", 2) { IsUnique = true }));

            //modelBuilder.Entity<GemeenteCategorie>().Ignore(x => x.categorieId);


            //modelBuilder
            //    .Entity<InspraakItem>()
            //    .HasOptional<GemeenteCategorie>(x => x.parentGemCat)
            //    .WithOptionalDependent()
            //;


            // Table per Type inheritance: 

            // modelBuilder.Entity<GemeenteCategorie>().ToTable("GemeenteCategories");
            //  modelBuilder.Entity<Actie>().ToTable("Acties");



            // Kan niet toegepast worden! Codes 0830 en 00 komen voor op meerdere niveaus!

            //modelBuilder
            //    .Entity<Categorie>()
            //    .Property(t => t.categorieCode)
            //    .IsRequired()
            //    .HasMaxLength(60)
            //    .HasColumnAnnotation(
            //    IndexAnnotation.AnnotationName,
            //    new IndexAnnotation(
            //    new IndexAttribute("IX_CatCode", 2) { IsUnique = true }));

        }



            public DbSet<Categorie> Categorien { get; set; }
            public DbSet<Actie> Acties { get; set; }
            public DbSet<InspraakItem> inspraakItems { get; set; }
            public DbSet<GemeenteCategorie> GemeenteCategorien { get; set; }
            public DbSet<HoofdGemeente> Gemeenten { get; set; }
            public DbSet<Politicus> Politici { get; set; }
            public DbSet<FinancieelOverzicht> FinancieleOverzichten { get; set; }
            public DbSet<Gebruiker> Gebruikers { get; set; }
            public DbSet<BegrotingsVoorstel> Voorstellen { get; set; }
            public DbSet<Project> Projecten { get; set; }
            public DbSet<BegrotingsVoorstelReactie> Reacties { get; set; }
            public DbSet<Account> Accounts { get; set; }
            public DbSet<Stem> Stemmen { get; set; }
            public DbSet<AntwoordEmail> AntwoordEmails { get; set; }
           // public DbSet<Cluster> Clusters { get; set; }

    }
}
