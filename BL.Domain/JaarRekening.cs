using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class JaarRekening : FinancieelOverzicht
    {
        public JaarRekening(int boekJaar, Gemeente gemeente) : base(boekJaar, gemeente) {}
        public JaarRekening() : base() { }

    }
}
