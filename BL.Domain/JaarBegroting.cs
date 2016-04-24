using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class JaarBegroting : FinancieelOverzicht
    {
        public JaarBegroting(int boekJaar, HoofdGemeente gemeente) : base(boekJaar, gemeente) { }
        public JaarBegroting() : base() { }
    }
}
