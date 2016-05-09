using BL;
using BL.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ActieController : ApiController
    {
        private ActieManager mgr = new ActieManager();

      /*  public IHttpActionResult Get(string catCode, string naam)
        {
            IEnumerable<DTOActie> acties = mgr.readActies(catCode, naam);
           /* if (acties == null || acties.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent); 


            return Ok(acties);
        } */
    }
}
