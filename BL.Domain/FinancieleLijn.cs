using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class FinancieleLijn
    {
        // public string bestuur { get; set; }
        [Key]
        public int ID { get; set; }
        public float inkomsten { get; set; }
        public float uitgaven { get; set; }
        public Actie actie { get; set; }
        public GemeenteCategorie cat { get; set; }
        public BestuurType bestuurType { get; set; }
        public FinancieelOverzicht financieelOverzicht { get; set; }

        public FinancieleLijn(float uitgaven, GemeenteCategorie cat)
    {
        this.uitgaven = uitgaven;
        this.cat = cat;
    }
    public FinancieleLijn(float uitgaven, GemeenteCategorie cat, Actie actie)
    {
        this.uitgaven = uitgaven;
        this.cat = cat;
        this.actie = actie;
    }
    public FinancieleLijn(float inkomsten, float uitgaven, GemeenteCategorie cat, Actie actie, BestuurType bt)
    {
        this.uitgaven = uitgaven;
        this.inkomsten = inkomsten;
        this.cat = cat;
        this.actie = actie;
        this.bestuurType = bt;
    }

        public FinancieleLijn(float inkomsten, float uitgaven, GemeenteCategorie cat, Actie actie, BestuurType bt, FinancieelOverzicht fo)
        {
            this.uitgaven = uitgaven;
            this.inkomsten = inkomsten;
            this.cat = cat;
            this.actie = actie;
            this.bestuurType = bt;
            this.financieelOverzicht = fo;
        }
        public FinancieleLijn()
        {

        }

        public override string ToString()
        {
            return String.Format("Uitgave: " + uitgaven + ", Categorie: " + cat.ToString() );
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
