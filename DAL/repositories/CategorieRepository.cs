using BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

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

        public CategorieRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }


        public Categorie CreateCategorie(Categorie cat)
        {
            ctx.Categorien.Add(cat);
            ctx.SaveChanges();

            return cat;
        }


        public Categorie ReadCategorie(string categoriecode)
        {
            return ctx.Categorien.Where<Categorie>(x => x.categorieCode == categoriecode).Single();
        }

        public IEnumerable<Categorie> ReadCategories()
        {
            return ctx.Categorien;
        }


        public GemeenteCategorie CreateIfNotExistsGemeenteCategorie(int catId, int foId, List<GemeenteCategorie> gemCats, List<Categorie> cats)
        {
            GemeenteCategorie gemCat = gemCats.Find(x => x.financieelOverzicht.Id == foId && x.categorieId == catId);

            if (gemCat == null)
            {
                Categorie c = cats.Find(x => x.categorieId == catId); 
                FinancieelOverzicht f = ctx.FinancieleOverzichten.Find(foId);

                // get parent id
                int? parentId = null;
                 if (c.categorieParent != null)
                {
                      parentId = (ReadGemeenteCategorie(c.categorieParent.categorieId, foId)).ID;
                }

                ctx.Entry(c).State = EntityState.Unchanged;
                gemCat = new GemeenteCategorie(c, f, parentId);
                return CreateGemeenteCategorie(gemCat);
            }
            
            return gemCat;
        }


        public GemeenteCategorie ReadGemeenteCategorie(string categorieCode, int foId)
        {
            return ctx.GemeenteCategorien.Where<GemeenteCategorie>(x => x.categorieCode == categorieCode).Where<GemeenteCategorie>(x => x.financieelOverzicht.Id == foId).SingleOrDefault();
        }

        public GemeenteCategorie ReadGemeenteCategorie(int categorieId, int foId)
        {
            return ctx.GemeenteCategorien.Where<GemeenteCategorie>(x => x.categorieId == categorieId).Where<GemeenteCategorie>(x => x.financieelOverzicht.Id == foId).SingleOrDefault();
        }

        public IEnumerable<GemeenteCategorie> ReadGemeenteCategories()
        {
            return ctx.GemeenteCategorien;
        }

        public IEnumerable<GemeenteCategorie> ReadGemeenteCategoriesWithFinOverzichten()
        {
            return ctx.GemeenteCategorien.Include(x=>x.financieelOverzicht);
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


        internal void UpdateGemeenteCatCumulative(GemeenteCategorie gemCat, float inkomsten, float uitgaven)
        {
            // inkomsten af trekken om te weten hoeveel de gemeente gaat uitgeven aan deze categorie
            gemCat.totaal += gemCat.calculateTotal(inkomsten, uitgaven);
            UpdateGemeenteCat(gemCat);
            return;
        }

        public void UpdateGemeenteCat(GemeenteCategorie gc)
        {
            ctx.Entry(gc).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public IEnumerable<InspraakItem> GetChildrenInspraakItem (InspraakItem ii)
        {
            return ctx.inspraakItems.Where<InspraakItem>(x => x.parentGemCatId == ii.ID);
        }

        public IEnumerable<InspraakItem> GetParentsInspraakItem(InspraakItem ii)
        {
            return ctx.inspraakItems.Where<InspraakItem>(x => x.ID == ii.parentGemCatId);
        }

        public IEnumerable<Categorie> GetParentsCategorie(Categorie cat)
        {
            return ctx.Categorien.Where<Categorie>(x => x.categorieId == cat.categorieParent.categorieId );
        }

        public IEnumerable<Categorie> GetClusterAverage(Cluster cluster, int jaar)
        {

            // Linq group by om de gemiddeldes snel te berekenen
            // Creatie van een GemeenteCategorie object om properties door te geven, mag zeker niet gepersisteerd worden
            var averages = from t in ctx.GemeenteCategorien.ToList()
                           join t2 in ctx.FinancieleOverzichten on t.financieelOverzicht.Id equals t2.Id
                           join t3 in ctx.Gemeenten.Include(x=>x.cluster) on t2.gemeente.HoofdGemeenteID equals t3.HoofdGemeenteID
                           where t3.cluster.clusterId == cluster.clusterId && t2.boekJaar == jaar
                           group t by new
                           {
                               t.categorieCode,t.categorieType, t.categorieNaam
                           }
                           into grp
                           select new Categorie()
                           {
                               totaal = grp.Average(p => p.totaal),
                               categorieCode = grp.Key.categorieCode,
                               categorieType = grp.Key.categorieType,
                               categorieNaam = grp.Key.categorieNaam
                           }
                       ;

            return averages;
        }

        public Categorie GetClosestParent(string code, string type)
        {
            // Eerst volledige categorie met parent opvissen aan de hand van code en type
            Categorie child = ctx.Categorien.Where<Categorie>(x => x.categorieType == type && x.categorieCode == code).Single();
            if (child.categorieParent != null)
            {
                return ctx.Categorien.Where<Categorie>(x => x.categorieId == child.categorieParent.categorieId).SingleOrDefault();
            }
            return default(Categorie);
        }




    }
}
