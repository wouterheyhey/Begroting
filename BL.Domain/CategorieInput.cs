using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class CategorieInput
    {
        public int Id { get; set; }
        public string type { get; set; }
        public string input { get; set; }
        public string icoon { get; set; }
        public byte [] foto { get; set; }
        public string film { get; set; }
        public GemeenteCategorie gemCategorie { get; set; }
        public string kleur { get; set; }


        public CategorieInput() { }

        public CategorieInput(string input, string icoon, string film, string foto, string kleur, string gem)
        {
            this.input = input;
            if (kleur != null)
                this.kleur = kleur;
            else
            {
                if (gem == "Algemene financiering")
                    this.kleur = "#999999";
                if (gem == "Algemeen bestuur")
                    this.kleur = "#cccccc";
                if (gem == "Zich verplaatsen en mobiliteit")
                    this.kleur = "#f7bdc7";
                if (gem == "Natuur en milieubeheer")
                    this.kleur = "#d0d257";
                if (gem == "Veiligheidszorg")
                    this.kleur = "#ff8e6c";
                if (gem == "Ondernemen en werken")
                    this.kleur = "#00cad2";
                if (gem == "Wonen en ruimtelijke ordening")
                    this.kleur = "#80d9be";
                if (gem == "Cultuur en vrije tijd")
                    this.kleur = "#ce205e";
                if (gem == "Leren en onderwijs")
                    this.kleur = "#fff66c";
                if (gem == "Zorg en opvang")
                    this.kleur = "#41526c";
            }

            if (icoon != null)
                this.icoon = icoon;
            else
            {
                if (gem == "Algemene financiering")
                    this.icoon = "glyphicon-euro";
                if (gem == "Algemeen bestuur")
                    this.icoon = "glyphicon-globe";
                if (gem == "Zich verplaatsen en mobiliteit")
                    this.icoon = "glyphicon-plane";
                if (gem == "Natuur en milieubeheer")
                    this.icoon = "glyphicon-tree-deciduous";
                if (gem == "Veiligheidszorg")
                    this.icoon = "glyphicon-warning-sign";
                if (gem == "Ondernemen en werken")
                    this.icoon = "glyphicon-lock";
                if (gem == "Wonen en ruimtelijke ordening")
                    this.icoon = "glyphicon-home";
                if (gem == "Cultuur en vrije tijd")
                    this.icoon = "glyphicon-glass";
                if (gem == "Leren en onderwijs")
                    this.icoon = "glyphicon-education";
                if (gem == "Zorg en opvang")
                    this.icoon = "glyphicon-heart";
            }

            if (film != null)
                this.film = film;

            if (foto != null)
                this.foto = stringConverter(foto);
        }

        private byte[] stringConverter(string beeld)
        {
            byte[] bytes = new byte[beeld.Length * sizeof(char)];
            System.Buffer.BlockCopy(beeld.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
