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
        public Gebruiker burger { get; set; }
        public Gebruiker moderator { get; set; }
    }
}
