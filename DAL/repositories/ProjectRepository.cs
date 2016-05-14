using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

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

        public void createProject(Project p, IDictionary<int,int> inspraakItems, int? boekjaar, string gemeente)
        {
            p.inspraakItems = new HashSet<InspraakItem>();

            if (inspraakItems != null)
            {
                //ID = key  &&  value = InspraakNiveau
                foreach (var item in inspraakItems)
                {
                    InspraakItem i = updateInspraakItem(item.Key, item.Value);
                    if (i != null)
                    {

                        p.inspraakItems.Add(i);
                    }

                }
            }


            //NOG EEN OPLOSSING ZOEKEN VOOR HET NIET BESTAAN VAN EEN BEGROTING EN ZORGEN DAT ER GEEN REKENING WORDT OPGEHAALD
            if (boekjaar != 0 && gemeente != null)
            {
                FinancieelOverzicht fov = ctx.FinancieleOverzichten.Where(fo => fo.gemeente.naam == gemeente && fo.boekJaar == boekjaar).SingleOrDefault();
                      p.begroting = (JaarBegroting)fov;
            }


            ctx.Projecten.Add(p);
            ctx.SaveChanges();
        }

        public InspraakItem updateInspraakItem(int id, int inspraakNiveau)
        {
            InspraakItem ii = ctx.inspraakItems.Find(id);


                ii.inspraakNiveau = (InspraakNiveau)inspraakNiveau;

            return ii;

        }

        public IEnumerable<InspraakItem> getInspraakItems(int jaar, string naam)
        {
            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == naam)
               .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
               .Select(c => c.Id).SingleOrDefault();


            //moeten het zo ophalen omdat dit multilevel recurieve objecten zijn
            var ip = from g in ctx.inspraakItems  where g.financieelOverzicht.Id == id  select g;   
               
            return ip;
        }

        

    }
}
