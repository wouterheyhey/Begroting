using BL.Domain;
using BL.Domain.DTOs;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
    public class AccountRepository : IDisposable
    {
        private AuthDBContext _ctx;
        private BegrotingDBContext ctx;

        private UserManager<IdentityUser> _userManager;

        public AccountRepository()
        {
            _ctx = new AuthDBContext();
            ctx = new BegrotingDBContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }
        
        //Gebruiker beheer - Identity User
        public async Task<IdentityResult> RegisterUser(DTOIngelogdeGebruiker aspGebruiker)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = aspGebruiker.email,
                Email = aspGebruiker.email,
            };

            var result = await _userManager.CreateAsync(user, aspGebruiker.Password);

            if (result == IdentityResult.Success)
            {
                _userManager.AddToRole(user.Id, RolType.standaard.ToString());
                IngelogdeGebruiker gebruiker = new IngelogdeGebruiker(2, aspGebruiker.Naam, aspGebruiker.email, null, true, RolType.moderator);
                ctx.Gebruikers.Add(gebruiker);
                ctx.SaveChanges();
            }
            return result;

        }
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }
        //Gebruiker beheer - Social User
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);
            return user;
        }
        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);
            return result;
        }
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);
            return result;
        }

        //Rol beheer
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_ctx));
            return rm.RoleExists(name);
        }
        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_ctx));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }

}
