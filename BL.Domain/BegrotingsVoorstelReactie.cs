using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class BegrotingsVoorstelReactie
    {
        public int Id { get; set; }
        public DateTime reactieDatum { get; set; }
        public string beschrijving { get; set; }
        public Gebruiker auteur { get; set; }
    }
}
