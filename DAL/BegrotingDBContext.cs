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
            public BegrotingDBContext() : base("BegrotingDB_EF")
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

            modelBuilder.Entity<FinancieelOverzicht>().Property(x => x.boekJaar).IsRequired();
            modelBuilder.Entity<FinancieelOverzicht>().HasRequired(x => x.gemeente);
            modelBuilder.Entity<FinancieelOverzicht>().ToTable("FinancieleOverzichten");

            modelBuilder.Entity<InspraakItem>().HasRequired(x => x.financieelOverzicht);
            modelBuilder.Entity<InspraakItem>().Property(x => x.inspraakNiveau).IsRequired();
            modelBuilder
                .Entity<InspraakItem>()
                .HasOptional<GemeenteCategorie>(x => x.parentGemCat)
                .WithMany(x => x.childInspraakItems)
                .HasForeignKey(fk => fk.parentGemCatId)
            ;

            modelBuilder.Entity<Actie>().Property(x => x.actieLang).HasMaxLength(180);
            modelBuilder.Entity<Actie>().Property(x => x.actieKort).HasMaxLength(60);
            modelBuilder.Entity<Actie>().Property(x => x.actieCode).HasMaxLength(15);

            modelBuilder.Entity<Cluster>().Property(x => x.name).IsRequired().HasMaxLength(120);

            modelBuilder.Entity<BegrotingsVoorstel>().ToTable("BegrotingsVoorstellen");
            modelBuilder.Entity<BudgetWijziging>().ToTable("BudgetWijzigingen");
            modelBuilder.Entity<Categorie>().ToTable("Categorien");
            modelBuilder.Entity<Politicus>().ToTable("Politici");
            modelBuilder.Entity<Project>().ToTable("Projecten");
            modelBuilder.Entity<Stem>().ToTable("Stemmen");
            modelBuilder.Entity<ProjectAfbeelding>().ToTable("ProjectAfbeeldingen");


            // Table per Type inheritance: 
            //modelBuilder.Entity<GemeenteCategorie>().ToTable("GemeenteCategorien");
            //modelBuilder.Entity<Actie>().ToTable("Acties");
            
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
            public DbSet<FAQ> FAQs { get; set; }
        // public DbSet<Cluster> Clusters { get; set; }

    }
}
