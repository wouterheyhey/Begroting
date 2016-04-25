using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;


namespace DAL
{
    public class CategoryRepository
    {
        private BegrotingDBContext ctx;

        public CategoryRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
        }


        public Categorie CreateCategorie(Categorie cat)
        {
            ctx.Categorien.Add(cat);
            ctx.SaveChanges();

            return cat; 
        }

        public Categorie ReadCategorie(string categorieCode)
        {
            return ctx.Categorien.Find(categorieCode);
        }

        public IEnumerable<Categorie> ReadCategories()
        {
            return ctx.Categorien;
        }

        public GemeenteCategorie CreateGemeenteCategorie(GemeenteCategorie cat)
        {
            ctx.GemeenteCategorien.Add(cat);
            ctx.SaveChanges();

            return cat;
        }

        public GemeenteCategorie ReadGemeenteCategorie(string categorieCode, string gemeenteNaam)
        {
            return ctx.GemeenteCategorien.Where<GemeenteCategorie>(x => x.cat.categorieCode == categorieCode).Where<GemeenteCategorie>(x => x.gemeente.naam == gemeenteNaam).SingleOrDefault();
        }

        // move to financial repo??
        public FinancieleLijn CreateFinLijn(FinancieleLijn finLijn)
        {
            ctx.FinLijnen.Add(finLijn);
            ctx.SaveChanges();

            return finLijn;
        }

        public void ImportFinancieleLijnen(int year)
        {
            string importPath = @"..\..\..\DAL\lib\";
            string categoryFile = "gemeente_categorie_acties_jaartal_uitgaven.xlsx";

            FinancieleLijn fn;
            int count =0 ;
            foreach (FinancieleLijn fl in
                ExcelImporter.ImportFinancieleLijnen(importPath + categoryFile, year, this))
            {
                fn = CreateFinLijn(fl);
                count++;
                Console.WriteLine(count + " Lines imported");
            }
            return;
        }

        // move to actie repo??
        public Actie ReadActie(string actieCode, string gemeenteNaam)
        {
            return ctx.Acties.Where<Actie>(x => x.actieCode == actieCode).Where<Actie>(x => x.gemeente.naam == gemeenteNaam).SingleOrDefault() ;
        }


        public Actie CreateActie(Actie actie)
        {
            ctx.Acties.Add(actie);
            ctx.SaveChanges();
            return actie;
        }

        public HoofdGemeente CreateGemeente(HoofdGemeente gemeente)
        {
            ctx.Gemeenten.Add(gemeente);
            ctx.SaveChanges();
            return gemeente;
        }

        public HoofdGemeente ReadGemeente(string gemeenteNaam)
        {
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten)).Where<HoofdGemeente>(x => x.naam == gemeenteNaam).SingleOrDefault();
        }

        public IEnumerable<HoofdGemeente>  ReadGemeentes()
        {
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten));
        }

        public List<Categorie> ReadChildrenCategories(Categorie cat)
        {
            // Subcategorien beginnen altijd met dezelfde categoriecode
            return ctx.Categorien.Where<Categorie>(x => x.categorieCode.StartsWith(cat.categorieCode) && x.categorieCode != cat.categorieCode).ToList<Categorie>();
        }


        public void UpdateAllCategoriesChildren()
        {
            foreach (Categorie cat in ReadCategories().ToList())
            {
                cat.categrorieChildren = ReadChildrenCategories(cat);
                // Voor debuggen
                 Console.WriteLine(cat.categrorieChildren.Count() + " added to " + cat.categorieCode);
            }
            ctx.SaveChanges();

        }

        public FinancieelOverzicht ReadFinancieelOverzicht(int jaar, HoofdGemeente gemeente)
        {
            return ctx.FinancieleOverzichten.Where<FinancieelOverzicht>(x => x.boekJaar == jaar).Where<FinancieelOverzicht>(x => x.gemeente.naam == gemeente.naam).SingleOrDefault();
        }

        public JaarBegroting CreateJaarBegroting(JaarBegroting jaarBegroting)
        {
            ctx.FinancieleOverzichten.Add(jaarBegroting);
            ctx.SaveChanges();
            return jaarBegroting;
        }

        public JaarRekening CreateJaarBegroting(JaarRekening jaarRekening)
        {
            ctx.FinancieleOverzichten.Add(jaarRekening);
            ctx.SaveChanges();
            return jaarRekening;
        }

        //public Cluster ReadCluster(string clusterName)
        //{
        //    return ctx.Clusters.Where<Cluster>(x => x.name == clusterName).SingleOrDefault();
        //}

    }
}
