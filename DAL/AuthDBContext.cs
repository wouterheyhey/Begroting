using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AuthDBContext : IdentityDbContext<IdentityUser>
    {
        public AuthDBContext()
            : base("BegrotingDB_Azure")
        {

        }
    }


}
