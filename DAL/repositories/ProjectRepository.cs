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

        public IEnumerable<InspraakItem> getInspraakItems(int jaar, string naam)
        {
            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == naam)
               .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
               .Select(c => c.Id).SingleOrDefault();


            //moeten het zo ophalen omdat dit multilevel recurieve objecten zijn
            var ip = from g in ctx.inspraakItems  where g.financieelOverzicht.Id == id  select g;   
               
            return ip;
        }

        /*   public void createProject(Project p, List<DTOGemeenteCategorie> inspraakItems, int? boekjaar, string gemeente)
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

             FinancieelOverzicht fov = ctx.FinancieleOverzichten.Where(fo => fo.gemeente.naam == gemeente && fo.boekJaar == boekjaar).SingleOrDefault();

              p.begroting = (JaarBegroting)fov;

              }

              ctx.Projecten.Add(p);
              ctx.SaveChanges();
          } */

    }
}
