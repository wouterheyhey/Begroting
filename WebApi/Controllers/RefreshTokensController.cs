using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using BL.Domain;
using Microsoft.Owin.Security;
using WebApi.Results;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApi.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;

namespace WebApi.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {

        private AccountManager accMgr = null;

        public RefreshTokensController()
        {
            accMgr = new AccountManager();
        }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(accMgr.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await accMgr.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                accMgr.Dispose();
            }

            base.Dispose(disposing);
        }
    }


}
