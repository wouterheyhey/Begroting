using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BL.Domain;
using BL;

namespace UI_CA
{
    class Program
    {

        static void Main(string[] args)
        {
            FinancieleLijnManager FinMgr = new FinancieleLijnManager();
            CategorieManager mgr = new CategorieManager();

            mgr.SetChildrenCategorien(); // geen invloed op EF
            FinMgr.LoadFinancieleLijnen(2020);
            /*
            FinMgr.LoadFinancieleLijnen(2019);
            FinMgr.LoadFinancieleLijnen(2018);
            FinMgr.LoadFinancieleLijnen(2017);
            FinMgr.LoadFinancieleLijnen(2016);
            FinMgr.LoadFinancieleLijnen(2015);
            */


            //     Console.ReadLine();
        }
    }


}
