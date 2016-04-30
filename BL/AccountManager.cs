using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;
using BL.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BL
{
    public class AccountManager
    {
        private AccountRepository repo;

        public AccountManager()
        {
            repo = new AccountRepository();
        }

        public Task<IdentityResult> RegisterUser (Account account)
        {
            return repo.RegisterUser(account);
        }

        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
