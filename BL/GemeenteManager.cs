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

        public HoofdGemeente GetGemeente(string name)
        {
            return repo.ReadGemeente(name);
        }
    }
}
