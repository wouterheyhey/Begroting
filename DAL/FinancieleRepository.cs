using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace DAL
{
    public class FinancieleRepository
    {
        private BegrotingDBContext ctx;



        public FinancieleLijn CreateCategorie(FinancieleLijn finLijn)
        {
            ctx.FinLijnen.Add(finLijn);
            ctx.SaveChanges();

            return finLijn;
        }
    }
}
