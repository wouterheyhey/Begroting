using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
   public class GemeenteRepository
    {
        private BegrotingDBContext ctx;

        public GemeenteRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        public HoofdGemeente CreateGemeente(HoofdGemeente gemeente)
        {
            ctx.Gemeenten.Add(gemeente);
            ctx.SaveChanges();
            return gemeente;
        }

        public HoofdGemeente ReadGemeente(string gemeenteNaam)
        {
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten)).Where<HoofdGemeente>(x => x.naam == gemeenteNaam).SingleOrDefault();
        }

        public IEnumerable<HoofdGemeente> ReadGemeentes()
        {
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten));
        }


    }
}
