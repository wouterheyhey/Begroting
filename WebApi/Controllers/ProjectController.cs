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
                 p.minBedrag, p.minBedrag, p.cats, p.boekjaar, p.gemeente); 
              return Ok();
          }

        //ophalen van project met de inspraakitems erbij
        public IHttpActionResult Get(int jaar, string gemeente)
        {
            Project p= mgr.getProject(jaar, gemeente);

            List<DTOGemeenteCategorie> gemCat = new List<DTOGemeenteCategorie>();

            if (p.inspraakItems != null)
            {
                foreach (var item in p.inspraakItems)
                {
                    gemCat.Add(new DTOGemeenteCategorie()
                    {
                        //naamCatz = item.gemCat.cat.categorieNaam,
                        naamCatz = "test",
                        inspraakNiveau = (int?)item.inspraakNiveau,
                        totaal = item.totaal
                    });
                }
            }

            DTOProject pr = new DTOProject()
            {
               projectScenario= (int)p.projectScenario,
               titel = p.titel,
                vraag= p.vraag,
                extraInfo = p.extraInfo,
                bedrag = p.bedrag,
                minBedrag = p.minBedrag,
                maxBedrag = p.maxBedrag,
                cats= gemCat,
       
    };
            return Ok(pr);
        }




    }




}
