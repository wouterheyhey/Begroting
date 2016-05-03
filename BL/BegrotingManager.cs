﻿using System;
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

        public IEnumerable<DTOfinancieleLijn> readFinancieleLijnen(int jaar, string naam )
        {
            return repo.GetFinancieleLijnen(jaar, naam);
        }

        public void addFinancieleLijnen(IEnumerable<FinancieleLijn> lijnen)
        {
            repo.CreateFinancieleLijnen(lijnen);
            return ;
        }
       
        // Nodig?
        public void LoadFinancieleLijnen(int year)
        {
       //     repo.CreateFinancieleLijnen(lijnen);
            return ;
        }





    }
}
