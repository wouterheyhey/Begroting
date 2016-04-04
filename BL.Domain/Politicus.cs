using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Politicus
    {
        public int PoliticusId { get; set; }
        public string naam { get; set; }
        public PoliticusType type { get; set; }
    }
}
