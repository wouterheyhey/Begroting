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
    public FinancieleLijn(float inkomsten, float uitgaven, GemeenteCategorie cat, Actie actie)
    {
        this.uitgaven = uitgaven;
        this.inkomsten = inkomsten;
        this.cat = cat;
        this.actie = actie;
    }
        public override string ToString()
        {
            return String.Format("Uitgave: " + uitgaven + ", Categorie: " + cat.ToString() );
        }
    }
}
