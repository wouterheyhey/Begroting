using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Desactivatie
    {
        public DateRange range { get; set; }
        public string reden { get; set; }
        public IngelogdeGebruiker burger { get; set; }
        public IngelogdeGebruiker moderator { get; set; }
    }
}
