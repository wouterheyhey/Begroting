using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTOs
{
    public class DTOActie
    {
        
            public int ID { get; set; }
            public string actieKort { get; set; }
            public string actieLang { get; set; }
            public float uitgaven { get; set; }
            public int inspraakNiveau { get; set; }

    }
}