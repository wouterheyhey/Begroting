using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BL.Domain;
using DAL.repositories;

namespace BL
{
    public class BegrotingManager
    {
        private readonly BegrotingRepository repo;
        public BegrotingManager()
        {
           repo = new BegrotingRepository();
        }

        public IEnumerable<GemeenteCategorie> readGemeenteCategories(int jaar, string naam)
        {
            return repo.getGemeenteCategories(jaar, naam);
        }

        public IEnumerable<JaarBegroting> readBegrotingen(string naam)
        {
            return repo.getBegrotingen(naam);
        }

        public IEnumerable<Actie> readActies(int id)
        {
            return repo.GetActies(id);
        }

        public int changeGemcatInput(int GemcatId, string input, string icoon, string film, string foto, string kleur)
        {
           return repo.updateGemcatInput(GemcatId, input, icoon, film, foto, kleur);
        }
    }
}
