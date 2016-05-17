using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string naam { get; set; }
        public string email { get; set; }
        public HoofdGemeente gemeente { get; set; }
        public RolType rolType { get; set; }
        public bool isActief { get; set; }

        //public HashSet<Account> accounts { get; set; }

        public Gebruiker()
        {

        }

        public Gebruiker(string email, string naam, string accountID, RolType roltype, HoofdGemeente gemeente)
        {
            this.userName = email;
            this.naam = naam;
            this.email = email;
            this.gemeente = gemeente;
            this.rolType = rolType;
        }
    }
}
