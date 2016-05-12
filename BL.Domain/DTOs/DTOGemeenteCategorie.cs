using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain.DTOs
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

        
    }
}
