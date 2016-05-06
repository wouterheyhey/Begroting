using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class IngelogdeGebruiker
    {
        public int Id { get; set; }
        public string naam { get; set; }
        public string accountID { get; set; }
        public string passwoord { get; set; }
        public HoofdGemeente gemeente { get; set; }
        public bool isActief { get; set; }
        public RolType rolType { get; set; }
        //public HashSet<Account> accounts { get; set; }

        public IngelogdeGebruiker(int Id, string naam, string accountID, string passwoord, bool isActief, RolType roltype)
        {
            this.Id = Id;
            this.naam = naam;
            this.accountID = accountID;
            this.passwoord = passwoord;
            this.gemeente = null;
            this.isActief = isActief;
            this.rolType = rolType;
            //this.gemeente = new HoofdGemeente("Brussel");
        }
    }
}
