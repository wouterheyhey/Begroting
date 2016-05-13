using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class AuthDBContext : IdentityDbContext<IdentityUser>
    {
        internal AuthDBContext()
            : base("BegrotingDB_Azure")
        {

        }
    }


}
