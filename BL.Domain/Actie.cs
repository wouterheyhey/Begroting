using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Actie : BegrotingsItem
    {
        public string actieCode { get; set; }
        public string actieKort { get; set; }
        public string actieLang { get; set; }
    }
}
