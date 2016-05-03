using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain.DTOs
{
    public class DTOfinancieleLijn
    {
        public DTOfinancieleLijn()
        {

        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string naamCatx { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string naamCaty { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string naamCatz { get; set; }

        public float uitgave { get; set; }

        public string catCode { get; set; }

    }
}
