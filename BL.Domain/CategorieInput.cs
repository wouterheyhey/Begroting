﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    class CategorieInput
    {
        public string type { get; set; }
        public string input { get; set; }
        public byte[] foto { get; set; }  
        public byte[] film { get; set; }
        public GemeenteCategorie gemCategorie { get; set; }
    }
}
