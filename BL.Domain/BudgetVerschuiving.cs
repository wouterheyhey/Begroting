using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class BudgetVerschuiving
    {
        public int ID { get; set; }
        public float bedrag { get; set; }
        public string beschrijving { get; set; }
        public InspraakItem inspraakItem { get; set; }
    }
}
