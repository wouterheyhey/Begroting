using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
    public class CategorieRepository
    {
        private BegrotingDBContext ctx;

        public CategorieRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
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





    }
}
