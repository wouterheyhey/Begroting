using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Actie : InspraakItem
    {
        //[Unique]
        public string actieCode { get; set; }
        public string actieKort { get; set; }
        public string actieLang { get; set; }

        public Actie()
        {
            this.inspraakNiveau = InspraakNiveau.Auto;
        }  // EF needs this

        public Actie(string actieCode, string actieKort, string actieLang)
        {
            this.actieCode = actieCode;
            this.actieKort = actieKort;
            this.actieLang = actieLang;
            this.inspraakNiveau = InspraakNiveau.Auto;
        }

        public Actie(string actieCode, string actieKort, string actieLang, HoofdGemeente gemeente)
        {
            this.actieCode = actieCode;
            this.actieKort = actieKort;
            this.actieLang = actieLang;
            this.gemeente = gemeente;
            this.inspraakNiveau = InspraakNiveau.Auto;
        }
    }


}
