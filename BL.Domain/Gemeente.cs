using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Gemeente
    {
        public int GemeenteID { get; set; }
        public string naam { get; set; }
        public int postCode { get; set; }
        public string provincie { get; set; }
        public HoofdGemeente hoofdGemeente { get; set; }

        public Gemeente() { }
        public Gemeente(string naam,int postcode)
        {
            this.naam = naam;
            this.postCode = postcode;
        }
    }
}
