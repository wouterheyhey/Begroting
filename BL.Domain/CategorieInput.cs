using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class CategorieInput
    {
        public int Id { get; set; }
        public string type { get; set; }
        public string input { get; set; }
        public byte[] icoon { get; set; }
        public byte[] foto { get; set; }
        public byte[] film { get; set; }
        public GemeenteCategorie gemCategorie { get; set; }
        public string kleur { get; set; }
    }
}
