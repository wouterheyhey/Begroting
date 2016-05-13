using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class FinancieleLijnImport
    {

        internal string gemeente { get; set; }
        internal float inkomsten { get; set; }
        internal float uitgaven { get; set; }
        internal string actieCode { get; set; }
        internal string actieKort { get; set; }
        internal string actieLang { get; set; }
        internal Dictionary<string,string> categorien { get; set; }
        internal string bestuur { get; set; }
        internal int jaar { get; set; }

        internal FinancieleLijnImport()
            {
            }

        internal FinancieleLijnImport(string groep,string bestuur, string actieKort, string actieLang,int jaar, float uitgaven, float inkomsten   )
        {
            this.gemeente = groep;
            this.bestuur = bestuur;        
            this.actieKort = actieKort;
            this.actieLang = actieLang;
            this.jaar = jaar;
            this.inkomsten = inkomsten;
            this.uitgaven = uitgaven;
            this.categorien = new Dictionary<string, string>();
        }
     }
}
