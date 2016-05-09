using BL.Domain;
using BL.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
    public class ActieRepository
    {
        private BegrotingDBContext ctx;

        public ActieRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

      public IEnumerable<DTOActie> GetActie(int id)
        {
            return ctx.Acties.Where(a => a.gemCat.ID == id).Select(

                fin => new DTOActie()
                {
                    actieKort = fin.actieKort,
                    actieLang = fin.actieLang
                }

                ).Distinct();
        } 
    }
}
