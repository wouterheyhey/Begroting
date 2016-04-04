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
        public Gemeente gemeente { get; set; }
        public IEnumerable<FinancieleLijn> lijnen { get; set; }

        public FinancieelOverzicht(int boekJaar, Gemeente gemeente)
        {
            this.boekJaar = boekJaar;
            this.gemeente = gemeente;
        }
        public FinancieelOverzicht()
        {

        }

    }
}
