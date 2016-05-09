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
        public BestuurType bestuurType { get; set; }
        public float inkomsten { get; set; }
        public float uitgaven { get; set; }
        //verplaatsen naar inspraakItem
        

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

        public Actie(string actieKort, string actieLang, BestuurType bt, float inkomsten, float uitgaven, FinancieelOverzicht fo, GemeenteCategorie gemCat)
        {
            this.actieKort = actieKort;
            this.actieLang = actieLang;
            this.inspraakNiveau = InspraakNiveau.Auto;
            this.bestuurType = bt;
            this.inkomsten = inkomsten;
            this.uitgaven = uitgaven;
            this.financieelOverzicht = fo;
            this.gemCat = gemCat;
        }

        public static BestuurType MapBestuurType(string bt)
        {
            if (bt.Contains("OCMW")) { return BestuurType.OCMW; }
            if (!bt.Contains(" "))  // Moet later beter uitgewerkt worden
            {
                return BestuurType.Gemeente;
            }
            else return BestuurType.AutonoomGemeenteBedrijf;

        }
    }


}
