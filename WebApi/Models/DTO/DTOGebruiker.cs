using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.DTO
{
    public class DTOGebruiker
    {
        public string userId { get; set; }
        public string naam { get; set; }
        public string gemeente { get; set; }
        public RolType rolType { get; set; }
        public bool isActief { get; set; }

        public DTOGebruiker(string email, string naam, string gemeente, RolType rolType, bool isActief)
        {
            this.userId = email;
            this.naam = naam;
            this.gemeente = gemeente;
            this.rolType = rolType;
            this.isActief = isActief;
        }
    }
}
