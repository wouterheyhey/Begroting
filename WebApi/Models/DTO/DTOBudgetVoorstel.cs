﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
{
    public class DTOBudgetVoorstel
    {
        public int aantalStemmen { get; set; }
        public string indiening { get; set; } //string datum
        public int totaal { get; set; }
        public string samenvatting { get; set; }
        public string beschrijving { get; set; }
        public List<DTOBudgetWijziging> budgetWijzigingen { get; set; }
        public int verificatieStatus { get; set; }
        public string verificatieDatum { get; set; } //string datum
        public string verificatorEmail { get; set; } 
        public string auteurEmail { get; set; } 

    }
}