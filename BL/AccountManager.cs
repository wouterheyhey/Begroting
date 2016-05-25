using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;
using BL.Domain;
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
        public Task<IdentityResult> RegisterUser(InTeLoggenGebruiker gebruiker)
        {
            GemeenteRepository repoGem = new GemeenteRepository();
            Task<IdentityResult> res = repo.RegisterUser(gebruiker, repoGem.ReadGemeente(gebruiker.gemeente));
            return res;
        }
        public Task<IdentityUser> FindUser(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return repo.FindUser(context.UserName, context.Password);
        }
        public Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            return repo.FindAsync(loginInfo);
        }
        public Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            return repo.CreateAsync(user);
        }
        public Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo)
        {
            return repo.AddLoginAsync(userId, loginInfo);
        }
        public Client FindClient(string clientId)
        {
            return repo.FindClient(clientId);
        }
        public Task<bool> AddRefreshToken(RefreshToken token)
        {
            return repo.AddRefreshToken(token);
        }
        public string GetAspUserId(string userName)
        {
            return repo.GetAspUserId(userName);
        }
        public Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return repo.RemoveRefreshToken(refreshToken);
        }
        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            return repo.RemoveRefreshToken(refreshTokenId);
        }
        public Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return repo.FindRefreshToken(refreshTokenId);
        }
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return repo.GetAllRefreshTokens();
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
        public Gebruiker GetGebruiker(string userName)
        {
            return repo.GetGebruiker(userName);
        }
        public IEnumerable<Gebruiker> GetGebruikers(string gemeente)
        {
            return repo.GetGebruikers(gemeente);
        }
        public bool ChangeGebruikers(List<Gebruiker> gebruikers)
        {
            return repo.UpdateGebruikers(gebruikers);
        }
        public RolType GetRole(string userName)
        {
            return repo.GetRole(userName);
        }

        public void Dispose()
        {
            repo.Dispose();
        }
    }
}
