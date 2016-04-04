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
            CategoryManager mgr = new CategoryManager();
            mgr.SetChildrenCategorien(); // geen invloed op EF
            mgr.LoadFinancieleLijnen(2020);
       //     Console.ReadLine();
        }
    }


}
