using BL.Domain;
using BL.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
    public class ProjectRepository
    {
        private BegrotingDBContext ctx;

        public ProjectRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        public void createProject(Project p, List<DTOGemeenteCategorie> inspraakItems, int? boekjaar, string gemeente)
        {
            p.inspraakItems = new HashSet<InspraakItem>();

            if (inspraakItems != null)
            {
                foreach (var item in inspraakItems)
                {
                    InspraakItem i = updateInspraakItem(item);
                    if (i != null)
                    {
                        
                        p.inspraakItems.Add(i);
                    }
                        
                }
            }

            if(boekjaar !=0 && gemeente !=null)
            {
                FinancieelOverzicht fov = ctx.FinancieleOverzichten.Where(fo => fo.gemeente.naam == gemeente && fo.boekJaar == boekjaar).SingleOrDefault();
             /*   if(fov != null)
                {
                    p.fo = fov;

                } */

            }
            
            
            ctx.Projecten.Add(p);
            ctx.SaveChanges();
        }

        public InspraakItem updateInspraakItem(DTOGemeenteCategorie i)
        {
            InspraakItem ii = ctx.inspraakItems.Find(i.ID);

            //aangezien DTOGemeenteCategorie een nullabale inspraakNiveau heeft (nodig voor frontend)
            // zal frontend een nulll hiervoor terugsturen als we deze categorie zijn inspraakniveau
            //niet wijzigen --> terug op Auto zetten.
            if(i.inspraakNiveau == null)
            ii.inspraakNiveau = InspraakNiveau.Auto;
            else
                ii.inspraakNiveau = (InspraakNiveau)i.inspraakNiveau;
            return ii;
        }

        public IEnumerable<DTOGemeenteCategorie> getInspraakItems(int jaar, string naam)
        {


            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == naam)
                .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
                .Select(c => c.Id).SingleOrDefault();

            //1 lijn --> 1 InspraakItem(GemeenteCategorie)  --> 1 Parent -> 1 Parent
            return ctx.GemeenteCategorien.Include(fin1 => fin1.cat.categorieParent.categorieParent)
                .Where<GemeenteCategorie>(fin1 => fin1.financieelOverzicht.Id == id).Select(

                fin2 => new DTOGemeenteCategorie()
                {
                    naamCatx = fin2.cat.categorieParent.categorieParent.categorieNaam,
                    naamCaty = fin2.cat.categorieParent.categorieNaam,
                    naamCatz = fin2.cat.categorieNaam,
                    totaal = fin2.totaal,
                    ID = fin2.ID
                }

                );
        }

        //ophalen van project met de inspraakitems erbij
        public Project readProject(int jaar, string gemeente)
        {
            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == gemeente)
                .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
                .Select(c => c.Id).SingleOrDefault();

            return ctx.Projecten.Include(x => x.inspraakItems).Where(p => p.Id == 1).SingleOrDefault();
        }

      
        public void saveContext()
        {
            ctx.SaveChanges();
        }
    }
}
