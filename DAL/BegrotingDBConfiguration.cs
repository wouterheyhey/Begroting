using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.DAL.EF
{
    class BegrotingDBConfiguration : DbConfiguration
    {
        public BegrotingDBConfiguration()
        {
            this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
            this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);

            this.SetDatabaseInitializer<BegrotingDBContext>(new BegrotingDBInitializer());

        }
    }
}
