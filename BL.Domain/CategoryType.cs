using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    // Category hierarchy depends on having the highest int for the lowest level in the hierarchy 
    public enum CategoryType
    { 
        A=1, 
        B=2, 
        C=3
    };
}
