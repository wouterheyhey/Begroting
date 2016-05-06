using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;
using BL.Domain.DTOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;

namespace BL
{
    public class AccountManager
    {
        AccountRepository repo;

        public AccountManager()
        {
            repo = new AccountRepository();
        }

        public Task<IdentityResult> RegisterUser(DTOIngelogdeGebruiker gebruiker)
        {
            Task<IdentityResult> res = repo.RegisterUser(gebruiker);
            return res;
        }

        public Task<IdentityUser> FindUser(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return repo.FindUser(context.UserName, context.Password);
        }

        public bool CreateRole(String roleName)
        {
            if (repo.RoleExists(roleName))
            {
                return false;
            }
            else
            {
                repo.CreateRole(roleName);
                return true;
            }
        }

        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
