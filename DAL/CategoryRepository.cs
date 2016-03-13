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
        }
}
