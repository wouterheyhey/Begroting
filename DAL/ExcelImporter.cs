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

        public static Dictionary<string, Categorie> ImportCategories(string path)
        {
            Dictionary<string,Categorie> hmap = new Dictionary<string,Categorie>();

            // Eventueel error throwen hier
            if (!File.Exists(path)) return default(Dictionary<string,Categorie>); 
            var book = new LinqToExcel.ExcelQueryFactory(path);

            foreach (string cat in Enum.GetNames(typeof(CategoryType)))
            {
               // book.AddMapping<Categorie>(x => x.categorieNaam, "Categorie " + cat);
                char charcat = cat.ToCharArray()[0]; // enum contains string, not char

                var rows = from c in book.Worksheet("Sheet1")
                           select c
                    ;

                string[] split;
                Categorie parent;
                foreach (var r in rows)  //needs parameterless constructor
                {
                    split = r["Categorie " + cat].Cast<string>().Split(new char[] { ' ' }, 2);
                    switch (charcat)
                    {
                        case 'B':
                            parent = hmap[r["Categorie " + 'A'].Cast<string>().Split(' ')[0]];
                            break;
                        case 'C':
                            parent = hmap[r["Categorie " + 'B'].Cast<string>().Split(' ')[0]];
                            break;
                        default:
                            parent = null;
                            break;
                    }


                    if (!hmap.ContainsKey(split[0]))
                    {
                        hmap.Add(split[0], new Categorie(split[0], split[1], charcat, parent));  //creating new category objects with data
                    }
                }
            }

            return hmap;
        }
    }
}
