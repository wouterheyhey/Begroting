using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
{
    public class DTOBegroting
    {
        public bool hasProject { get; set; }
        public int boekjaar { get; set; }
        public List<DTOGemeenteCategorie> childCats { get; set; }

    }
}