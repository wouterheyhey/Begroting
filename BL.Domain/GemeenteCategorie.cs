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
        }
        public GemeenteCategorie(Categorie cat, Gemeente gemeente)
        {
            this.cat = cat;
            this.gemeente = gemeente;
        }

        public GemeenteCategorie()  // Required by EF
        {
        }

    }
}
