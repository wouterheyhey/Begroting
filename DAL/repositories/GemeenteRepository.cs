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

        public GemeenteRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
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
            return ctx.Gemeenten.Include(nameof(HoofdGemeente.deelGemeenten)).Include(nameof(HoofdGemeente.FAQs)).Include(nameof(HoofdGemeente.bestuur)).Where<HoofdGemeente>(x => x.naam == gemeenteNaam).SingleOrDefault();
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

            if (bestuur != null)
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


        public void DeleteBestuurlid(int id)
        {
            Politicus p = ctx.Politici.Find(id);
            ctx.Politici.Remove(p);
            ctx.SaveChanges();
        }

        public void DeleteFAQ(int id)
        {
            FAQ f = ctx.FAQs.Find(id);
            ctx.FAQs.Remove(f);
            ctx.SaveChanges();
        }

        public int UpdateGemeenteInput(int id, HashSet<FAQ> faqs, string hoofdkleur, string logo)
        {
           HoofdGemeente g =  ctx.Gemeenten.OfType<HoofdGemeente>().Include(f => f.FAQs).Where(i => i.HoofdGemeenteID == id).SingleOrDefault();

            if(g != null)
            {
                g.hoofdKleur = hoofdkleur;

                if(logo != null)
                {
                    byte[] bytes = new byte[logo.Length * sizeof(char)];
                    System.Buffer.BlockCopy(logo.ToCharArray(), 0, bytes, 0, bytes.Length);
                    g.logo = bytes;
                }
                

                if (faqs != null)
                {
                    foreach (var item in faqs)
                    {
                        if (item.id == 0)
                        {
                            g.FAQs.Add(item);

                        }
                    }
                }

                ctx.SaveChanges();
                return g.HoofdGemeenteID;
            }

            return 0;

        }


        public void SaveContext()
        {
            ctx.SaveChanges();
        }

        public Cluster GetCluster(int id)
        {
            return ctx.Clusters.Find(id);
        }
    }
}
