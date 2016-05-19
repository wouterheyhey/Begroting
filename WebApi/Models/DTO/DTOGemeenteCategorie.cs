using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
{
    public class DTOGemeenteCategorie
    {
        public int ID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string catA { get; set; }
     //   public Dictionary<string, string> cats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string catB { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string catC { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? inspraakNiveau { get; set; }

        public float totaal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DTOActie> acties { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? gemcatID { get; set; }


    }
}