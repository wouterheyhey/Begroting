using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BL.Domain
{
    public class HoofdGemeente
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
        public HashSet<Gemeente> deelGemeenten { get; set; }
        public Cluster cluster { get; set; }

        public HoofdGemeente(string naam)
        {
            this.naam = naam;
        }
        public HoofdGemeente(string naam, string provincie, Cluster cluster)
        {
            this.naam = naam;
            this.provincie = provincie;
            this.cluster = cluster;
        }
        public HoofdGemeente(string naam, string provincie, Cluster cluster, int postCode)
        {
            this.naam = naam;
            this.provincie = provincie;
            this.cluster = cluster;
            this.postCode = postCode;
        }
        public HoofdGemeente(string naam, string provincie, int postCode)
        {
            this.naam = naam;
            this.provincie = provincie;
            this.postCode = postCode;
        }
        public HoofdGemeente(string naam, Cluster cluster)
        {
            this.naam = naam;
            this.cluster = cluster;
        }
        public HoofdGemeente()
        {
        }
        public override string ToString()
        {
            return String.Format("Gemeente: " + naam);
        }
    }
}
