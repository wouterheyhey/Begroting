using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BL.Domain
{
    public class GemeenteCategorie : InspraakItem
    {
       // public Categorie cat { get; set; }
        public int categorieId { get; set; }
        public string categorieCode { get; set; }
        public string categorieNaam { get; set; }
        public string categorieType { get; set; }   // char do not get mapped in EF !!! -> keep string
        public ICollection<InspraakItem> childInspraakItems { get; set; }  
        public CategorieInput categorieInput { get; set; }


        public GemeenteCategorie(Categorie cat)
        {
         //   this.cat = cat;
            this.inspraakNiveau = InspraakNiveau.Open;
            this.categorieId = cat.categorieId;
            this.categorieCode = cat.categorieCode;
            this.categorieNaam = cat.categorieNaam;
            this.categorieType = cat.categorieType;
        }
        public GemeenteCategorie(Categorie cat, FinancieelOverzicht fo)
        {
          //  this.cat = cat;
            this.financieelOverzicht = fo;
            this.inspraakNiveau = InspraakNiveau.Open;
            this.categorieId = cat.categorieId;
            this.categorieCode = cat.categorieCode;
            this.categorieNaam = cat.categorieNaam;
            this.categorieType = cat.categorieType;
            this.parentGemCatId = cat.categorieParent != null ?
                 cat.categorieParent.categorieId :
                 this.parentGemCatId = null;
        }
        public GemeenteCategorie(Categorie cat, FinancieelOverzicht fo, int? gemCatId)
        {
            //  this.cat = cat;
            this.financieelOverzicht = fo;
            this.inspraakNiveau = InspraakNiveau.Open;
            this.categorieId = cat.categorieId;
            this.categorieCode = cat.categorieCode;
            this.categorieNaam = cat.categorieNaam;
            this.categorieType = cat.categorieType;
            this.parentGemCatId = gemCatId;
        }


        public GemeenteCategorie()  // Required by EF
        {
            this.inspraakNiveau = InspraakNiveau.Open;
        }

        public float calculateTotal(float inkomsten, float uitgaven)
        {
            return (uitgaven - inkomsten);
        }

    }
}
