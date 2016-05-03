using BL.Domain.DTOs;
using DAL.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ActieManager
    {
        ActieRepository repo;

        public ActieManager()
        {
            repo = new ActieRepository();
        }
        public IEnumerable<DTOActie> readActies(string catCode, string naam)
        {
            return repo.GetActie(catCode, naam);
        }

    }
}
