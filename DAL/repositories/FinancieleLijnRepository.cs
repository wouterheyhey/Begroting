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


        internal CategoryType FindLowestNonBlankCategoryType(FinancieleLijnImport r)
        {
            // looping over enum elements in descending order
            foreach (CategoryType catType in ((CategoryType[])Enum.GetValues(typeof(CategoryType))).OrderByDescending(x => x))
            {
                if (r.categorien[catType.ToString()].Length > 0)
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
                gem = gems.Find(x => x.naam.Equals(r.gemeente));
                fo = begRepo.CreateIfNotExistsFinancieelOverzicht(year, gem, fos);
                fos.Add(fo);
                ct = FindLowestNonBlankCategoryType(r);

                foreach (CategoryType catType in Enum.GetValues(typeof(CategoryType)))
                {
                    cat=cats.Where(x => x.categorieType == catType.ToString()).Where(x => x.categorieCode.Equals(r.categorien[catType.ToString()].Split(new char[] { ' ' })[0])).SingleOrDefault();

                    gemCat = catRepo.CreateIfNotExistsGemeenteCategorie(cat.categorieId, fo.Id, gemCats, cats);
                    gemCats.Add(gemCat);
                    catRepo.UpdateGemeenteCatCumulative(gemCat, r.inkomsten, r.uitgaven);

                    if ( catType == ct)  
                        {
                        bt = Actie.MapBestuurType(r.bestuur);

                        actie = begRepo.CreateIfNotExistsActie(r.actieKort, r.actieLang, acties, bt,fo, gemCat.ID);
                        acties.Add(actie);
                        begRepo.UpdateActieCumulative(actie, r.inkomsten, r.uitgaven);
                        
                    }
                }

               
            }

            catRepo.SaveContext();
            return inspraakItems;
        }
    }
}
