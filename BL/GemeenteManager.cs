using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;
using BL.Domain;

namespace BL
{
   public class GemeenteManager
    {
        GemeenteRepository repo;

        public GemeenteManager()
        {
            repo = new GemeenteRepository();
        }

        public IEnumerable<HoofdGemeente> GetGemeenten()
        {
            return repo.ReadGemeentes();
        }

        public HoofdGemeente GetGemeente(string naam)
        {
            return repo.ReadGemeente(naam);
        }

        public void ChangeGemeente(string naam, int aantalBewoners, int opp, string maat, float man, float vrouw, float kind, HashSet<Politicus> bestuur, float aanslagvoet)
        {
            repo.UpdateGemeente(naam, aantalBewoners, opp, maat, man, vrouw, kind, bestuur, aanslagvoet);
        }

        public void DeleteBestuurlid(int id)
        {
            repo.DeleteBestuurlid(id);
        }

        public void DeleteFAQ(int id)
        {
            repo.DeleteFAQ(id);
        }

        public int ChangeGemeenteInput(int id, HashSet<FAQ> faqs, string hoofdkleur, string logo)
        {
            return repo.UpdateGemeenteInput(id, faqs, hoofdkleur, logo);
        }

        public Cluster ReadCluster(int id)
        {
            return repo.GetCluster(id);
        }
    }
}
