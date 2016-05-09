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

     /*   public IEnumerable<DTOActie> GetActie(string catCode, string naam)
        {
            return ctx.FinLijnen.Include(nameof(FinancieleLijn.actie)).Where(p => p.cat.cat.categorieCode == catCode && p.cat.gemeente.naam == naam).Select(

                fin => new DTOActie()
                {
                    actieKort = fin.actie.actieKort,
                    actieLang = fin.actie.actieLang
                }

                ).Distinct();
        } */
    }
}
