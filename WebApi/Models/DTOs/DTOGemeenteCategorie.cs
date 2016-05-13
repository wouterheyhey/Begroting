using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTOs
{
    public class DTOGemeenteCategorie
    {
        public int ID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string naamCatx { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string naamCaty { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string naamCatz { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? inspraakNiveau { get; set; }

        public float totaal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DTOActie> acties { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? gemcatID { get; set; }

    

    }
}