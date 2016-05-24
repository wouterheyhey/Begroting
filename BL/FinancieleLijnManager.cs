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
            finRepo = new FinancieleLijnRepository();
        }
        public FinancieleLijnManager(UnitOfWorkManager uofMgr)
        {
            finRepo = new FinancieleLijnRepository(uofMgr.UnitOfWork);
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
