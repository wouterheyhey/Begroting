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


        public CategoryType FindLowestNonBlankCategoryType(LinqToExcel.Row row)
        {
            // looping over enum elements in descending order
            foreach (CategoryType catType in ((CategoryType[])Enum.GetValues(typeof(CategoryType))).OrderByDescending(x => x))
            {
                if (row["categorie " + catType.ToString()].Cast<string>().Length > 0)
                {
                    return catType;
                }
            }
            // return highest if all are blank
            return Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().Max();
        }

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
            CategoryType ct;
            Categorie cat;

            // Single calls to the DB instead of repeating for each loop
            List<HoofdGemeente> gems = gemRepo.ReadGemeentes().ToList<HoofdGemeente>();
            List<GemeenteCategorie> gemCats = catRepo.ReadGemeenteCategories().ToList<GemeenteCategorie>();
            List<Categorie> cats = catRepo.ReadCategories().ToList<Categorie>();
            List<Actie> acties = begRepo.ReadActies().ToList<Actie>();
            List<FinancieelOverzicht> fos = begRepo.ReadFinancieelOverzichten().ToList<FinancieelOverzicht>();

            foreach (var r in ExcelImporter.ImportFinancieleLijnen(importPath + categoryFile, year))
            {
                gem = gems.Find(x => x.naam.Equals(r["Groep"].Cast<string>()));
                fo = begRepo.CreateIfNotExistsFinancieelOverzicht(year, gem, fos);
                fos.Add(fo);
                ct = FindLowestNonBlankCategoryType(r);

                foreach (CategoryType catType in Enum.GetValues(typeof(CategoryType)))
                {
                    cat=cats.Where(x => x.categorieType == catType.ToString()).Where(x => x.categorieCode.Equals(r["categorie " + catType.ToString()].Cast<string>().Split(new char[] { ' ' })[0])).SingleOrDefault();

                    gemCat = catRepo.CreateIfNotExistsGemeenteCategorie(cat.categorieId, fo.Id, gemCats, cats); // lijn hangen aan laagste hierarchieniveau
                    gemCats.Add(gemCat);

                    if( catType == ct)  
                        {
                        bt = Actie.MapBestuurType(r["Naam bestuur"].Cast<string>());

                        actie = begRepo.CreateIfNotExistsActie(r["Actie kort"].Cast<string>(), r["Actie lang"].Cast<string>(), acties, bt, r["Bedrag ontvangst per niveau"].Cast<float>(), r["Bedrag uitgave per niveau"].Cast<float>(), fo, gemCat.ID);
                        acties.Add(actie);
                    }
                }

               
                // inspraakItems.Add(new Actie(r["Bedrag ontvangst per niveau"].Cast<float>(), r["Bedrag uitgave per niveau"].Cast<float>(), gemCat, actie, bt, fo));  //creating new category objects with data
            }

            catRepo.SaveContext();
            return inspraakItems;
        }
    }
}
