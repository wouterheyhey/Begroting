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

        public IEnumerable<DTOfinancieleLijn> readFinancieleLijnen(int jaar, int gemeenteId )
        {
            return repo.GetFinancieleLijnen(jaar, gemeenteId);
        }

       

        public void LoadFinancieleLijnen(int year)
        {
            repo.ImportFinancieleLijnen(year);
        }

        public void SetChildrenCategorien()
        {
            repo.UpdateAllCategoriesChildren();
        }


    }
}
