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

       public IEnumerable<DTOGemeenteCategorie> readInspraakItems(int jaar, string naam )
        {
            return repo.getInspraakItems(jaar, naam);
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
