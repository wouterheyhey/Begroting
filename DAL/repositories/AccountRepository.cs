using BL.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
    public class AccountRepository
    {
        private AuthDBContext ctx;
        private UserManager<IdentityUser> usrMgr;

        public AccountRepository()
        {
            ctx = new AuthDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        public async Task<IdentityResult> RegisterUser(Account userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.AccountId
            };

            var result = await usrMgr.CreateAsync(user, userModel.AccountPassword);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await usrMgr.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            try
            {
                ctx.Dispose();
                usrMgr.Dispose();
            }
            catch(Exception e)
            {
                //Toegevoegd omdat null ref optrad bij usrMgr
            }


        }


    }
}
