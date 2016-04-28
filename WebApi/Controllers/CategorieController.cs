using BL;
using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class CategorieController : ApiController
    {
        private CategorieManager mgr = new CategorieManager();
        

        // GET: api/Category
         public IHttpActionResult Get()
        {
            var categorien = mgr.GetCategorien();

            if (categorien == null || categorien.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(categorien);
        } 

       



    }
}
