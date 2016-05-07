using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace DAL.repositories
{
    public class FinancieleLijnRepository
    {
        private readonly BegrotingRepository begRepo = new BegrotingRepository();
        private readonly CategorieRepository catRepo = new CategorieRepository();
        private readonly GemeenteRepository gemRepo = new GemeenteRepository();

        public List<FinancieleLijn> ImportFinancieleLijnen() { return null; }  // Uit te werken

        public List<FinancieleLijn> ImportFinancieleLijnen(int year)
        {
            List<FinancieleLijn> lines = new List<FinancieleLijn>();
            string categoryFile = "gemeente_categorie_acties_jaartal_uitgaven.xlsx";
            string importPath = (new FileLocator()).findExcelSourceDir();
            GemeenteCategorie cat;
            Actie actie;
            HoofdGemeente gem;
            BestuurType bt;
            FinancieelOverzicht fo;

            List<HoofdGemeente> gems = gemRepo.ReadGemeentes().ToList<HoofdGemeente>();
            List<GemeenteCategorie> gemCats = catRepo.ReadGemeenteCategories().ToList<GemeenteCategorie>();
            List<Categorie> cats = catRepo.ReadCategories().ToList<Categorie>();
            List<Actie> acties = begRepo.ReadActies().ToList<Actie>();
            List<FinancieelOverzicht> fos = begRepo.ReadFinancieelOverzichten().ToList<FinancieelOverzicht>();
            
            foreach (var r in ExcelImporter.ImportFinancieleLijnen(importPath + categoryFile, year))  
            {

                    gem = gems.Find(x => x.naam.Equals(r["Groep"].Cast<string>()));

                    //gem = gemRepo.ReadGemeente(r["Groep"].Cast<string>());
                    // cat = catRepo.CreateIfNotExistsGemeenteCategorie(r["Categorie C"].Cast<string>().Split(new char[] { ' ' })[0], gem); // lijn hangen aan laagste hierarchieniveau
                    cat = catRepo.CreateIfNotExistsGemeenteCategorie(r["Categorie C"].Cast<string>().Split(new char[] { ' ' })[0], gem, gemCats, cats); // lijn hangen aan laagste hierarchieniveau
                    gemCats.Add(cat);
                    
                    actie = begRepo.CreateIfNotExistsActie(r["Actie code"].Cast<string>(), r["Actie kort"].Cast<string>(), r["Actie lang"].Cast<string>(), gem, acties);
                    acties.Add(actie);
                    
                    fo = begRepo.CreateIfNotExistsFinancieelOverzicht(year, gem, fos);
                    fos.Add(fo);

                    bt = FinancieleLijn.MapBestuurType(r["Naam bestuur"].Cast<string>());
                    lines.Add(new FinancieleLijn(r["Bedrag ontvangst per niveau"].Cast<float>(), r["Bedrag uitgave per niveau"].Cast<float>(), cat, actie, bt, fo));  //creating new category objects with data

            }

            catRepo.SaveContext();
            return lines;
        }
    }
}
