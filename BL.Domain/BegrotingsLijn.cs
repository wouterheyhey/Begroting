using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class BegrotingsLijn
    {
        public string bestuur { get; set; }
        public float inkomsten { get; set; }
        public float uitgaven { get; set; }
        public Actie actie { get; set; }
    }
}
