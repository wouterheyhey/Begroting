using BL;
using BL.Domain;
using BL.Domain.DTOs;
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
            // GET: api/Gemeente/id
            public IHttpActionResult Get(int id)
            {
                var gemeente = mgr.GetGemeente(id);
                if(gemeente==null)
                    return StatusCode(HttpStatusCode.NoContent);

                return Ok(gemeente);
            } 

    }
}
