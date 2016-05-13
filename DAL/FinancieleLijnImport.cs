using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FinancieleLijnImport
    {

        public string gemeente { get; set; }
        public float inkomsten { get; set; }
        public float uitgaven { get; set; }
        public string actieCode { get; set; }
        public string actieKort { get; set; }
        public string actieLang { get; set; }
        public Dictionary<string,string> categorien { get; set; }
        public string bestuur { get; set; }
        public int jaar { get; set; }

        public FinancieleLijnImport()
            {
            }

        public FinancieleLijnImport(string groep,string bestuur, string actieKort, string actieLang,int jaar, float uitgaven, float inkomsten   )
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
