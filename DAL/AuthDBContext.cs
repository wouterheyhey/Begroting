using BL.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }


}
