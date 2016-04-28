using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class GemeenteController : ApiController
    {
        private GemeenteManager mgr = new GemeenteManager();

        // GET: api/Gemeente
        public IHttpActionResult Get()
        {
            var gemeenten = mgr.GetGemeenten();

            if (gemeenten == null || gemeenten.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(gemeenten);
        }
        // GET: api/Gemeente?name=Brussel
        public IHttpActionResult Get(string name)
        {
            var gemeente = mgr.GetGemeente(name);
            if(gemeente==null)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(gemeente);
        }
    }
}
