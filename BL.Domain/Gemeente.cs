using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BL.Domain
{
    public class Gemeente
    {
        [Key]
        public int GemeenteID { get; set; }
        public string naam { get; set; }
        public int postCode { get; set; }
        public string provincie { get; set; }
        public int aantalBewoners { get; set; }
        public int oppervlakte { get; set; }
        public string oppervlakteMaat { get; set; }
        public float isMan { get; set; }
        public float isVrouw { get; set; }
        public float isKind { get; set; }
        public HashSet<Politicus> bestuur { get; set; }
        public float aanslagVoet { get; set; }
        public Gemeente parent { get; set; }
        //public GemeenteType type;

        public Gemeente(string naam)
        {
            this.naam = naam;
        }
        public Gemeente()
        {
        }
        public override string ToString()
        {
            return String.Format("Gemeente: " + naam);
        }
    }
}
