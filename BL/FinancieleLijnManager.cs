using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL.repositories;


namespace BL
{
    public class FinancieleLijnManager
    {
        private readonly BegrotingManager begManager;
        private FinancieleLijnRepository  finRepo;

        public FinancieleLijnManager()
        {
            this.finRepo = new FinancieleLijnRepository();
            this.begManager = new BegrotingManager();
        }


        public void LoadFinancieleLijnen(int year)
        {
            List<InspraakItem> lijnen = finRepo.ImportFinancieleLijnen(year); 
        }


        public void LoadFinancieleLijnen()
        {
            finRepo.ImportFinancieleLijnen();
        }
    }
}
