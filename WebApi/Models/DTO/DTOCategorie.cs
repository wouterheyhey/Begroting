using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
    {
        public class DTOCategorie
        {
            public int ID { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string naamCat { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string catA { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string catB { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string catC { get; set; }

            public float totaal { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string catCode { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string catType { get; set; }



        }
    }
