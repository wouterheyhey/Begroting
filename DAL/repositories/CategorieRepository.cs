using BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return ctx.Categorien.Where<Categorie>(x => x.categorieCode == categorieCode).Single();
        }

        public IEnumerable<Categorie> ReadCategories()
        {
            return ctx.Categorien;
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


        public List<Categorie> ReadChildrenCategories(Categorie cat)
        {
            // Subcategorien beginnen altijd met dezelfde categoriecode
            return ctx.Categorien.Where<Categorie>(x => x.categorieCode.StartsWith(cat.categorieCode) && x.categorieCode != cat.categorieCode).ToList<Categorie>();
        }

        public GemeenteCategorie CreateIfNotExistsGemeenteCategorie(int catId, int foId, List<GemeenteCategorie> gemCats, List<Categorie> cats)
        {
            GemeenteCategorie gemCat = gemCats.Find(x => x.financieelOverzicht.Id == foId && x.categorieId == catId);


            if (gemCat == null)
            {
                Categorie c = cats.Find(x => x.categorieId == catId); // Exception for cat not found? 
                FinancieelOverzicht f = ctx.FinancieleOverzichten.Find(foId);

                // get parent id
                int? parentId = null;
                 if (c.categorieParent != null)
                {
                    parentId = (ReadGemeenteCategorie(c.categorieParent.categorieId, foId)).ID;
                }

                ctx.Entry(c).State = EntityState.Unchanged;
                gemCat = new GemeenteCategorie(c, f, parentId);

                //      ctx.Entry(gemCats).State = EntityState.Unchanged; // werkt niet
                //if (gemCat.parentGemCat != null)
                //{
                //    ctx.Entry(gemCat.parentGemCat).State = EntityState.Unchanged;
                //}
                return CreateGemeenteCategorie(gemCat);
            }
            
            return gemCat;
        }


        // Geeft geen unieke gemeentecategorien terug!
        public GemeenteCategorie ReadGemeenteCategorie(string categorieCode, int foId)
        {
            return ctx.GemeenteCategorien.Where<GemeenteCategorie>(x => x.categorieCode == categorieCode).Where<GemeenteCategorie>(x => x.financieelOverzicht.Id == foId).SingleOrDefault();
        }

        public GemeenteCategorie ReadGemeenteCategorie(int categorieId, int foId)
        {
            return ctx.GemeenteCategorien.Where<GemeenteCategorie>(x => x.categorieId == categorieId).Where<GemeenteCategorie>(x => x.financieelOverzicht.Id == foId).SingleOrDefault();
        }

        /*  public GemeenteCategorie ReadGemeenteCategorie(string categorieCode, string gemeenteNaam)
          {
              return ctx.GemeenteCategorien.Where<GemeenteCategorie>(x => x.cat.categorieCode == categorieCode).Where<GemeenteCategorie>(x => x.gemeente.naam == gemeenteNaam).SingleOrDefault();
          } */


        public IEnumerable<GemeenteCategorie> ReadGemeenteCategories()
        {
            return ctx.GemeenteCategorien;
        }


        public GemeenteCategorie CreateGemeenteCategorie(GemeenteCategorie cat)
        {
            ctx.GemeenteCategorien.Add(cat);
            ctx.SaveChanges();

            return cat;
        }

        public GemeenteCategorie CreateNoSaveGemeenteCategorie(GemeenteCategorie cat)
        {
            ctx.GemeenteCategorien.Add(cat);
            return cat;
        }

        public void SaveContext()
        {
            ctx.SaveChanges();
        }







    }
}
