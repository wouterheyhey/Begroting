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
    public class BegrotingController : ApiController
    {
        private BegrotingManager mgr = new BegrotingManager();

        public IHttpActionResult Get(int jaar, int gemeenteId)
        {
            IEnumerable<DTOfinancieleLijn> lijnen = mgr.readFinancieleLijnen(jaar, gemeenteId);

            if (lijnen == null || lijnen.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);


            return Ok(lijnen);
        }

       
    }
}
