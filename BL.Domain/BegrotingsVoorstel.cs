using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Domain
{
    public class BegrotingsVoorstel
    {
        public int Id { get; set; }
        public int aantalStemmen { get; set; }
        public DateTime indiening { get; set; }
        public float totaal { get; set; }
        public string samenvatting { get; set; }
        [MaxLength(1000)]
        public string beschrijving { get; set; }
        public HashSet<BudgetWijziging> budgetWijzigingen { get; set; }
        public HashSet<Stem> stemmen { get; set; }
        public VerificatieStatus verificatieStatus { get; set; }
        public DateTime verificatieDatum { get; set; }
        public Gebruiker verificator { get; set; }
        public Gebruiker auteur { get; set; }
    }
}
