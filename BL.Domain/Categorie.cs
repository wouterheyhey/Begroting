using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Domain
{
    public class Categorie
    {
        public int categorieId { get; set; }
        public string categorieCode { get; set; }
        public string categorieNaam { get; set; }
        public string categorieType { get; set; }   // char do not get mapped in EF !!! -> keep string
        public Categorie categorieParent { get; set; }
        public List<Categorie> categrorieChildren { get; set; }

        public Categorie(string categorieCode, string categorieNaam, string categorieType, Categorie categorieParent = null)
        {
            this.categorieCode = categorieCode;
            this.categorieNaam = categorieNaam;
            this.categorieParent = categorieParent;
            this.categorieType = categorieType;

        }
        // Parameterless constructor for linqtoexcel
        public Categorie()
        {

        }


        public override string ToString()
        {
            return String.Format(categorieCode + ", " + categorieNaam + ", " + categorieType + "; Parent: " + ((categorieParent != null) ? categorieParent.ToString() : "Geen")); 
        }

    }
}
