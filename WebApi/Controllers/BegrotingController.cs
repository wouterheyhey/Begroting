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
        private BegrotingManager begMgr = new BegrotingManager();
        private CategorieManager catMgr = new CategorieManager();
        private FinancieleLijnManager finMgr = new FinancieleLijnManager();

        public IHttpActionResult Get(int jaar, string naam)
        {
            IEnumerable<DTOGemeenteCategorie> lijnen = begMgr.readInspraakItems(jaar, naam);

            if (lijnen == null || lijnen.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(lijnen);

        } 

        public IHttpActionResult Get(int year)
        {
            try
            {
                catMgr.SetChildrenCategorien();
                finMgr.LoadFinancieleLijnen(year);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.OK);

        }

        public IHttpActionResult Get()
        {
            try
            {
                catMgr.SetChildrenCategorien();
                finMgr.LoadFinancieleLijnen();
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.OK);
        }
    }
}
