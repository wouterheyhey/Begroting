using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.DTO;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Gemeente")]
    public class GemeenteController : ApiController
    {

        // GET: api/Gemeente
        public IHttpActionResult Get()
        {
            GemeenteManager mgr = new GemeenteManager();
            var gemeenten = mgr.GetGemeenten();

            if (gemeenten == null || gemeenten.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(gemeenten);
        }
        // GET: api/Gemeente/id
        public IHttpActionResult Get(string naam)
        {
            GemeenteManager mgr = new GemeenteManager();
            var gemeente = mgr.GetGemeente(naam);
            if (gemeente == null)
                return StatusCode(HttpStatusCode.NoContent);
            return Ok(gemeente);
        }

        [HttpPut]
        public IHttpActionResult Put(DTOHoofdGemeente h)
        {
            GemeenteManager mgr = new GemeenteManager();
            mgr.ChangeGemeente(h.naam, h.aantalBewoners, h.oppervlakte, h.oppervlakteMaat, h.isMan, h.isVrouw,
                h.isKind, h.bestuur, h.aanslagVoet);

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Put(int id,DTOHoofdGemeente h)
        {
            GemeenteManager mgr = new GemeenteManager();
            int i = mgr.ChangeGemeenteInput(id, h.FAQs, h.hoofdkleur, h.logo);

            return Ok(i);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            GemeenteManager mgr = new GemeenteManager();
            mgr.DeleteBestuurlid(id);
            return Ok();
        }

        [Route("deleteFAQ/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteFAQ(int id)
        {
            GemeenteManager mgr = new GemeenteManager();
            mgr.DeleteFAQ(id);
            return Ok();
        }

    }
}
