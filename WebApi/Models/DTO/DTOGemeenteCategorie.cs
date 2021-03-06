﻿using Newtonsoft.Json;
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
        public string naamCat { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string catA { get; set; }

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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DTOGemeenteCategorie> childCats { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string catCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string catType { get; set; }

        public int? inputID { get; set; }

        public string input { get; set; }


        public string icoon { get; set; }

        public string foto { get; set; }

        public string film { get; set; }

        public string kleur { get; set; }



    }
}