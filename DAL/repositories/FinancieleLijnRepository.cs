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

        public List<InspraakItem> ImportFinancieleLijnen(int year)
        {
            List<InspraakItem> inspraakItems = new List<InspraakItem>();
            string categoryFile = "gemeente_categorie_acties_jaartal_uitgaven.xlsx";
            string importPath = (new FileLocator()).findExcelSourceDir();
            GemeenteCategorie gemCat;
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
                fo = begRepo.CreateIfNotExistsFinancieelOverzicht(year, gem, fos);
                fos.Add(fo);

                gemCat = catRepo.CreateIfNotExistsGemeenteCategorie(r["Categorie C"].Cast<string>().Split(new char[] { ' ' })[0], fo.Id, gemCats, cats); // lijn hangen aan laagste hierarchieniveau
                gemCats.Add(gemCat);
                bt = Actie.MapBestuurType(r["Naam bestuur"].Cast<string>());

                actie = begRepo.CreateIfNotExistsActie(r["Actie kort"].Cast<string>(), r["Actie lang"].Cast<string>(), acties, bt, r["Bedrag ontvangst per niveau"].Cast<float>(), r["Bedrag uitgave per niveau"].Cast<float>(), fo, gemCat.ID);
                acties.Add(actie);
                // inspraakItems.Add(new Actie(r["Bedrag ontvangst per niveau"].Cast<float>(), r["Bedrag uitgave per niveau"].Cast<float>(), gemCat, actie, bt, fo));  //creating new category objects with data
            }

            catRepo.SaveContext();
            return inspraakItems;
        }
    }
}
