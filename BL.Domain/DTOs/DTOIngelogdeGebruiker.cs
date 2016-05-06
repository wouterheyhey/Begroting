using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain.DTOs
{
    public class DTOIngelogdeGebruiker
    {

        [Required]
        public string Naam { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string accountID { get; set; }

        [Required]
        public string bevestigPaswoord { get; set; }

        [Required]
        public string email { get; set; }

        //public HoofdGemeente gemeente { get; set; }
        //public bool isActief { get; set; }

        //[Required]
        //public RolType rolType { get; set; }
        //public HashSet<Account> accounts { get; set; }
    }

}
