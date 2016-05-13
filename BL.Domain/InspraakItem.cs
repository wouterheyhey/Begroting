using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Domain
{
    public abstract class InspraakItem
    {
        public int ID { get; set; }
        public float totaal { get; set; }
      //  public float relatiefAandeel { get; set; }
        public byte[] logo { get; set; }  // byte[] as type for the image for EF
        public InspraakNiveau inspraakNiveau { get; set; }
        public FinancieelOverzicht financieelOverzicht { get; set; }

        [ForeignKey("parentGemCat")]
        public int? parentGemCatId { get; set; }
        public GemeenteCategorie parentGemCat { get; set; }

    }
}
