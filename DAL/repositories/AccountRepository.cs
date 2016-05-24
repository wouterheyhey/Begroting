using BL.Domain;
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
        public async Task<IdentityResult> RegisterUser(InTeLoggenGebruiker aspGebruiker, HoofdGemeente gem,  RolType rolType=RolType.standaard)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = aspGebruiker.email,
                Email = aspGebruiker.email,
                EmailConfirmed = true
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
                _userManager.AddToRole(user.Id, rolType.ToString());
                Gebruiker gebruiker = new Gebruiker(aspGebruiker.email, aspGebruiker.Naam, aspGebruiker.email, gem, rolType);
                ctx.Entry(gebruiker.gemeente).State = System.Data.Entity.EntityState.Unchanged;
                ctx.Gebruikers.Add(gebruiker);
                ctx.SaveChanges();
            }
            return result;

        }
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);
            if (!_userManager.IsLockedOut(GetAspUserId(userName))) return user;
            else return default(IdentityUser);
        }
        public Gebruiker GetGebruiker(string userName)
        {
            return ctx.Gebruikers.Include(nameof(Gebruiker.gemeente)).Where<Gebruiker>(x => x.userName == userName).SingleOrDefault();
        }
        public bool UpdateGebruikers(List<Gebruiker> gebruikers)
        {
            try
            {
                foreach (Gebruiker g in gebruikers)
                {
                    string aspUserId = GetAspUserId(g.userName);
                    Gebruiker gebruikerInContext = GetGebruiker(g.userName);
                    if (!bool.Equals(gebruikerInContext.isActief,g.isActief))
                    {
                        if (g.isActief)
                        {
                            EnableUser(gebruikerInContext, aspUserId);
                        }
                        else
                        {
                            DisableUser(gebruikerInContext, aspUserId);
                        }
                    }
                    if (!Enum.Equals(gebruikerInContext.rolType,g.rolType))
                    {
                        SetRole(g.rolType, gebruikerInContext, aspUserId);
                    }
                }
                _ctx.SaveChanges();
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Gebruiker> GetGebruikers(string gemeente)
        {
            return ctx.Gebruikers.Include(nameof(Gebruiker.gemeente)).Where(g => g.gemeente.naam == gemeente);
        }
        public bool DisableUser(Gebruiker g, string aspUserId)
        {
            try
            {
                _userManager.SetLockoutEnabled(aspUserId, true);
                _userManager.SetLockoutEndDate(aspUserId, DateTime.MaxValue);
                g.isActief = false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
        public bool EnableUser(Gebruiker g, string aspUserId)
        {
            try
            {
                _userManager.SetLockoutEnabled(aspUserId, false);
                _userManager.SetLockoutEndDate(aspUserId, DateTime.Now);
                g.isActief = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public RolType GetRole(string userName)
        {
            try
            {
                Gebruiker g = GetGebruiker(userName);
                return g.rolType;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetAspUserId(string username)
        {
            return _ctx.Users.Where(x => x.UserName == username).First().Id;
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
        public bool SetRole(RolType rolType, Gebruiker g, string aspUserId)
        {
            try
            {
                _userManager.RemoveFromRoles(aspUserId, _userManager.GetRoles(aspUserId).ToArray<string>());
                _userManager.AddToRole(aspUserId, rolType.ToString());
                g.rolType = rolType;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

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
