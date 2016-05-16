using BL;
using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.DTO;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Project")]
    public class ProjectController : ApiController
    {
        private ProjectManager mgr = new ProjectManager();
        private BegrotingManager begMgr = new BegrotingManager();
        [Route("itemsGET")]
        [HttpGet]
        public IHttpActionResult Get(int jaar, string naam)
        {
            IEnumerable<InspraakItem> lijnen = mgr.getInspraakItems(jaar, naam);

            if (lijnen == null || lijnen.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOGemeenteCategorie> gemcats = new List<DTOGemeenteCategorie>();

            foreach (var item in lijnen)
            {
                var gemCat = item as GemeenteCategorie;
                if (gemCat != null)
                {
                    gemcats.Add(new DTOGemeenteCategorie()
                    {

                        ID = gemCat.ID,
                        totaal = gemCat.totaal,
                        catC = gemCat.categorieNaam,
                        inspraakNiveau = (int?)gemCat.inspraakNiveau,
                        gemcatID = gemCat.parentGemCatId,
                    });

                }
            }
            foreach (var item in lijnen)
            {
                var actie = item as Actie;
                if (actie != null)
                {
                    var gemcat = gemcats.Where(x => x.ID == item.parentGemCatId).SingleOrDefault();

                    if (gemcat.acties == null)
                    {
                        gemcat.acties = new List<DTOActie>();
                    }
                    gemcat.acties.Add(new DTOActie()
                    {
                        actieKort = actie.actieKort,
                        actieLang = actie.actieLang,
                        inspraakNiveau = (int)actie.inspraakNiveau,
                        uitgaven = actie.uitgaven,
                        ID = actie.ID,
                        bestuurtype = (int)actie.bestuurType
                    });
                }
            }


            return Ok(gemcats);
        }

        [HttpPost]
        public IHttpActionResult Post(DTOProject p)
        {
            //K= id + V= inspraakNiveau
            IDictionary<int, int> inspraakItems = new Dictionary<int, int>();

            foreach (var item in p.cats)
            {
                inspraakItems.Add(new KeyValuePair<int, int>(item.ID, (int)item.inspraakNiveau));

                if (item.acties != null)
                {
                    foreach (var actie in item.acties)
                    {
                        inspraakItems.Add(new KeyValuePair<int, int>(actie.ID, actie.inspraakNiveau));
                    }
                }

            }

           int id =  mgr.addProject((ProjectScenario)p.projectScenario, p.titel, p.vraag, p.extraInfo, p.bedrag,
              p.minBedrag, p.minBedrag, inspraakItems, p.boekjaar, p.gemeente);

            if(id == 0)
                return BadRequest("Er is iets misgelopen bij het registreren van het project!");
            return Ok(id);
        }

        [Route("projectGET")]
        [HttpGet]
        public IHttpActionResult GetProject(int jaar, string naam)
        {

            Project p = mgr.getProject(jaar, naam);

            DTOProject dp = new DTOProject()
            {
                projectScenario = (int)p.projectScenario,
                titel = p.titel,
                vraag = p.vraag,
                extraInfo = p.extraInfo,
                bedrag = p.bedrag,
                minBedrag = p.minBedrag,
                maxBedrag = p.maxBedrag,
                boekjaar = (int?)jaar,
                gemeente = naam
            };
            return Ok(dp);
        }
        [HttpGet]
        public IHttpActionResult GetProjects( string naam)
        {
            IEnumerable<Project> p = mgr.getProjects( naam);

            if (p == null || p.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOProject> dp = new List<DTOProject>();
            foreach (var item in p)
            {
                dp.Add(new DTOProject()
                {
                    projectScenario = (int)item.projectScenario,
                    titel = item.titel,
                    vraag = item.vraag,
                    extraInfo = item.extraInfo,
                    bedrag = item.bedrag,
                    minBedrag = item.minBedrag,
                    maxBedrag = item.maxBedrag,
                    boekjaar = (int?)item.begroting.boekJaar,
                    gemeente = naam
                });
            }
            return Ok(dp);
        }



    }
}


