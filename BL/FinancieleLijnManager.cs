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
        private FinancieleLijnRepository finRepo;

        public FinancieleLijnManager()
        {
            this.finRepo = new FinancieleLijnRepository();
        }


        public void LoadFinancieleLijnen(string categoryFile, int year)
        {
            List<InspraakItem> lijnen = finRepo.ImportFinancieleLijnen(categoryFile, year);
        }


        public void LoadFinancieleLijnen()
        {
            finRepo.ImportFinancieleLijnen();
        }
    }

}
