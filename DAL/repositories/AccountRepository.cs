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
        private RoleManager<IdentityRole> _roleManager;

        public AccountRepository()
        {
            _ctx = new AuthDBContext();
            ctx = new BegrotingDBContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }
        
        //Gebruiker beheer - Identity User
        public async Task<IdentityResult> RegisterUser(DTOIngelogdeGebruiker aspGebruiker, HoofdGemeente gem)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = aspGebruiker.email,
                Email = aspGebruiker.email 
            };

            var result = await _userManager.CreateAsync(user, aspGebruiker.Password);

            if (result == IdentityResult.Success)
            {
                if (!RoleExists(RolType.standaard.ToString()))
                {
                    var values = Enum.GetValues(typeof(RolType));
                    foreach (var str in values)
                    {
                        if (!RoleExists(str.ToString())) CreateRole(str.ToString());
                    }
                }
                _userManager.AddToRole(user.Id, RolType.standaard.ToString());
                IngelogdeGebruiker gebruiker = new IngelogdeGebruiker(aspGebruiker.email, aspGebruiker.Naam, aspGebruiker.email, RolType.standaard, gem);
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
            return _roleManager.RoleExists(name);
        }
        public bool CreateRole(string name)
        {
            var idResult = _roleManager.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }

        //Client & refreshtoken beheer
        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }

}
