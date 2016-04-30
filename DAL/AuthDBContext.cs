using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL
{
    public class AuthDBContext : IdentityDbContext<IdentityUser>
    {
        public AuthDBContext() 
            : base ("BegrotingDB_EF")
        {

        }
    }
}
