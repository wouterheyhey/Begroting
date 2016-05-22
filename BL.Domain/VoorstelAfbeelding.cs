using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class VoorstelAfbeelding
    {
        public int ID { get; set; }
        public byte[] Afbeelding { get; set; }

        public VoorstelAfbeelding()
        {

        }
        public VoorstelAfbeelding(byte[] afbeelding)
        {
            this.Afbeelding = afbeelding;
        }
    }
}
