using BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten)).Include(nameof(HoofdGemeente.bestuur)).Where<HoofdGemeente>(x => x.naam == gemeenteNaam).SingleOrDefault();
        }

        // SPRINT2: wordt niet gebruikt
        public HoofdGemeente ReadGemeente(int id)
        {
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten)).Where<HoofdGemeente>(x => x.HoofdGemeenteID == id).SingleOrDefault();
        }

        public IEnumerable<HoofdGemeente> ReadGemeentes()
        {
            return ctx.Gemeenten;
        }


        public void UpdateGemeente(string naam, int aantalBewoners, int opp, string maat, float man, float vrouw, float kind, HashSet<Politicus> bestuur, float aanslagvoet)
        {

            HoofdGemeente g = ctx.Gemeenten.Include(nameof(HoofdGemeente.bestuur)).Where(x => x.naam == naam).SingleOrDefault();

            if (g.bestuur != null)
            {
                foreach (var item in bestuur)
                {
                    if (item.PoliticusId == 0)
                    {
                        g.bestuur.Add(item);

                    }
                }
            }

            g.aantalBewoners = aantalBewoners;
            g.oppervlakte = opp;
            g.oppervlakteMaat = maat;
            g.isMan = man;
            g.isVrouw = vrouw;
            g.isKind = kind;
            g.aanslagVoet = aanslagvoet;
            ctx.SaveChanges();
        }


        public void deleteBestuurlid(int id)
        {
            Politicus p = ctx.Politici.Find(id);
            ctx.Politici.Remove(p);
            ctx.SaveChanges();
        }


        public void saveContext()
        {
            ctx.SaveChanges();
        }
    }
}
