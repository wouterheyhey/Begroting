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
        public byte[] icoon { get; set; }
        public byte[] foto { get; set; }
        public byte[] film { get; set; }
        public GemeenteCategorie gemCategorie { get; set; }
        public string kleur { get; set; }


        public CategorieInput() { }

        public CategorieInput(string input, string icoon, string film, string foto, string kleur)
        {
            this.input = input;
            this.kleur = kleur;

            if (icoon != null)
                this.icoon = stringConverter(icoon);

            if (film != null)
                this.film = stringConverter(film);

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
