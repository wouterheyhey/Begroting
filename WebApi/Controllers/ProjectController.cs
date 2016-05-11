using BL;
using BL.Domain;
using BL.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.DTO;

namespace WebApi.Controllers
{
    public class ProjectController : ApiController
    {
        private ProjectManager mgr = new ProjectManager();
        private ActieManager Amgr = new ActieManager();

          [HttpPost] 
          public IHttpActionResult Post(DTOProject p)
          {
               mgr.addProject((ProjectScenario)p.projectScenario, p.titel, p.vraag, p.extraInfo, p.bedrag,
                 p.minBedrag, p.minBedrag, p.cats); 
              return Ok();
          } 


      

    }




}
