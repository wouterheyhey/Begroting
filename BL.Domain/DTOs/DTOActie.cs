using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain.DTOs
{
   public class DTOActie
    {
        public int ID { get; set; }
        public string actieKort { get; set; }
        public string actieLang { get; set; }
        public float uitgaven { get; set; }
        public InspraakNiveau inspraakNiveau { get; set; }
    }
}
