using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;

namespace BL
{
    public class AccountManager
    {
        AccountRepository repo;

        public AccountManager()
        {
            repo = new AccountRepository();
        }
    }
}
