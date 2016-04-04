using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace DAL
{
    public class ExcelImporter
    {
        public static IEnumerable<Gemeente> ImportGemeenten(string path)
        {
            List<Gemeente> gemeenten = new List<Gemeente>();

            // Eventueel error throwen hier
            if (!File.Exists(path)) return default(List<Gemeente>);
            var book = new LinqToExcel.ExcelQueryFactory(path);

            var rows = from c in book.Worksheet("Sheet1")
                       select c
                ;

            string gemeenteNaam;
            foreach (var r in rows)  //needs parameterless constructor
            {
                gemeenteNaam = r["GemeenteNaam"].Cast<string>();
                if (gemeenteNaam != null )  gemeenten.Add(new Gemeente(gemeenteNaam));
            }

            return gemeenten;
        }

        public static Dictionary<string, Categorie> ImportCategories(string path)
        {
            Dictionary<string,Categorie> hmap = new Dictionary<string,Categorie>();

            // Eventueel error throwen hier
            if (!File.Exists(path)) return default(Dictionary<string,Categorie>); 
            var book = new LinqToExcel.ExcelQueryFactory(path);

            foreach (string cat in Enum.GetNames(typeof(CategoryType)))
            {
                // book.AddMapping<Categorie>(x => x.categorieNaam, "Categorie " + cat);
                // char charcat = cat.ToCharArray()[0]; // enum contains string, not char
               // char do not get mapped in EF !!! -> keep string

                var rows = from c in book.Worksheet("Sheet1")
                           select c
                    ;

                string[] split;
                Categorie parent;
                foreach (var r in rows)  //needs parameterless constructor
                {
                    split = r["Categorie " + cat].Cast<string>().Split(new char[] { ' ' }, 2);
                    switch (cat)
                    {
                        case "B":
                            parent = hmap[r["Categorie " + 'A'].Cast<string>().Split(' ')[0]];
                            break;
                        case "C":
                            parent = hmap[r["Categorie " + 'B'].Cast<string>().Split(' ')[0]];
                            break;
                        default:
                            parent = null;
                            break;
                    }


                    if (!hmap.ContainsKey(split[0]))
                    {
                        hmap.Add(split[0], new Categorie(split[0], split[1], cat, parent));  //creating new category objects with data
                    }
                }
            }

            return hmap;
        }

        
        public static List<FinancieleLijn> ImportFinancieleLijnen(string path,int year, CategoryRepository catRepo)
        {
            List<FinancieleLijn> lines = new List<FinancieleLijn>();

            // Eventueel error throwen hier
            if (!File.Exists(path)) return default(List<FinancieleLijn>);
            var book = new LinqToExcel.ExcelQueryFactory(path);

            var rows = from c in book.Worksheet("Actie_detail_2")
                       where c["Financieel boekjaar"].Cast<int>() == year
                       select c;

            GemeenteCategorie cat;
            Actie actie;
            Gemeente gem;
            BestuurType bt;
            FinancieelOverzicht fo;

            foreach (var r in rows)  //needs parameterless constructor
            {
                cat = catRepo.ReadGemeenteCategorie(r["Categorie C"].Cast<string>().Split(new char[] { ' ' })[0], r["Groep"].Cast<string>()); // lijn hangen aan laagste hierarchieniveau
                actie = catRepo.ReadActie(r["Actie code"].Cast<string>(), r["Groep"].Cast<string>());
                gem = catRepo.ReadGemeente(r["Groep"].Cast<string>());
                fo = catRepo.ReadFinancieelOverzicht(year, gem);

                if (actie == null)
                {
                    actie = catRepo.CreateActie(new Actie(r["Actie code"].Cast<string>(), r["Actie kort"].Cast<string>(), r["Actie lang"].Cast<string>(),gem));
                }

                if (cat == null)
                {
                    cat = catRepo.CreateGemeenteCategorie(new GemeenteCategorie(catRepo.ReadCategorie(r["Categorie C"].Cast<string>().Split(new char[] { ' ' })[0]), gem));
                }


                if (fo == null)
                {
                    // logic to decide if begroting or rekening. beter ergens in een functie zetten
                    bool check = year <= DateTime.Now.Year ;
                    switch (check)
                        {
                        case true:
                            fo = catRepo.CreateJaarBegroting(new JaarBegroting(year, gem));
                            break;
                        case false:
                            fo = catRepo.CreateJaarBegroting(new JaarBegroting(year, gem));
                            break;
                    }
                         

                    
                }

                bt = FinancieleLijn.MapBestuurType(r["Naam bestuur"].Cast<string>());

                lines.Add(new FinancieleLijn(r["Bedrag ontvangst per niveau"].Cast<float>(), r["Bedrag uitgave per niveau"].Cast<float>(), cat, actie, bt, fo));  //creating new category objects with data
            }

            return lines;

        }


    }
}
