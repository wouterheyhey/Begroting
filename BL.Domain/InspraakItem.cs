using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public abstract class InspraakItem
    {
        [Key]
        public int ID { get; set; }
        public string inspraakMogelijk { get; set; }
        public float totaal { get; set; }
        public object logo { get; set; } 
        public Gemeente gemeente { get; set; }
    }
}
