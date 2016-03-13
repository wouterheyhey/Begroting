using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public abstract class BegrotingsItem
    {
        public string inspraakMogelijk { get; set; }
        public float totaal { get; set; }
    }
}
