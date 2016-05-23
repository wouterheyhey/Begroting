using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public abstract class FinancieelOverzicht
    {
        public int Id { get; set; }
        public int boekJaar { get; set; }
        public HoofdGemeente gemeente { get; set; }
        public ICollection<InspraakItem> lijnen { get; set; }

        public FinancieelOverzicht(int boekJaar, HoofdGemeente gemeente)
        {
            this.boekJaar = boekJaar;
            this.gemeente = gemeente;
        }
        public FinancieelOverzicht()
        {

        }

    }
}
