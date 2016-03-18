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

            public DbSet<Categorie> Categorien { get; set; }
            public DbSet<FinancieleLijn> FinLijnen { get; set; }
            public DbSet<Actie> Acties { get; set; }
            public DbSet<GemeenteCategorie> GemeenteCategorien { get; set; }
            public DbSet<Gemeente> Gemeenten { get; set; }

    }
}
