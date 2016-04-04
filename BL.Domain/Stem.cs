using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Stem
    {
        public int ID { get; set; }
        DateTime registratieDatum { get; set; }
         Account account { get; set; }
    }
}
