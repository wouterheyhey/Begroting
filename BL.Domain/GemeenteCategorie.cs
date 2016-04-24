using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BL.Domain
{
    public class GemeenteCategorie : InspraakItem
    {
        public Categorie cat { get; set; }

        public GemeenteCategorie(Categorie cat)
        {
            this.cat = cat;
            this.inspraakNiveau = InspraakNiveau.Auto;
        }
        public GemeenteCategorie(Categorie cat, HoofdGemeente gemeente)
        {
            this.cat = cat;
            this.gemeente = gemeente;
            this.inspraakNiveau = InspraakNiveau.Auto;
        }

        public GemeenteCategorie()  // Required by EF
        {
            this.inspraakNiveau = InspraakNiveau.Auto;
        }

    }
}
