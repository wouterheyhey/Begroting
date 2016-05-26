using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
{
    public class DTOBegrotingVoorstel
    {
        public int Id { get; set; }
        public int aantalStemmen { get; set; }
        public float totaal { get; set; }
        public string samenvatting { get; set; }
        public string beschrijving { get; set; }
        public List<DTOBudgetWijziging> budgetWijzigingen { get; set; }
        public List<DTOGemeenteCategorie> gemcats { get; set; }
        public int verificatieStatus { get; set; }
        public string verificatorEmail { get; set; } 
        public string auteurEmail { get; set; }
        //dit veld nodig voor Angular 2 waybinding
        public string reactie { get; set; }
        public List<DTOReactie> reacties { get; set; }
        public List<string> afbeeldingen { get; set; }
        public string auteurNaam { get; set; }

    }
}