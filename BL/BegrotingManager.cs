using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BL.Domain;
using DAL.repositories;
using BL.Domain.DTOs;

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

        public IEnumerable<Actie> readActies(int id)
        {
            return repo.GetActies(id);
        }

        /*
        public void addFinancieleLijnen(IEnumerable<FinancieleLijn> lijnen)
        {
            repo.CreateFinancieleLijnen(lijnen);
            return ;
        }
       
        // Nodig?
        public void LoadFinancieleLijnen(int year)
        {
            //repo.CreateFinancieleLijnen(year);
            return ;
        } */

    }
}
