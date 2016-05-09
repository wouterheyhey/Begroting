using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public abstract class InspraakItem
    {
        [Key]
        public int ID { get; set; }
        public float totaal { get; set; }
        public float relatiefAandeel { get; set; }
        public byte[] logo { get; set; }  // byte[] as type for the image for EF
       // public HoofdGemeente gemeente { get; set; }
        public InspraakNiveau inspraakNiveau { get; set; }
        //deze weglaten
        public FinancieelOverzicht financieelOverzicht { get; set; }
        public GemeenteCategorie gemCat { get; set; }

    }
}
