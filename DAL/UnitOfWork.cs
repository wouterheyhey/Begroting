using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class UnitOfWork
    {
        private BegrotingDBContext ctx;
        internal BegrotingDBContext Context
        {
            get
            {
                if (ctx == null) ctx = new BegrotingDBContext(true);
                return ctx;
            }
        }
        public void CommitChanges()
        {
            ctx.CommitChanges();
        }
    }
}
