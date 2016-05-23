using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
{
    public class DTOHoofdGemeente
    {
        public int HoofdGemeenteID { get; set; }
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
        public HashSet<FAQ> FAQs { get; set; }
        public string hoofdkleur { get; set; }
        public string logo { get; set; }
    }
}