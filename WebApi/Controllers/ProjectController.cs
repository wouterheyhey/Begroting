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

            return Ok(convertInspraakItems(lijnen));
           
        }


        [Route("postProject")]
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
              p.minBedrag, p.minBedrag, inspraakItems, p.boekjaar, p.gemeente, p.isActief, p.afbeeldingen);

            if(id == 0)
                return BadRequest("Er is iets misgelopen bij het registreren van het project!");
            return Ok(id);
        }

        [Route("projectGET")]
        [HttpGet]
        public IHttpActionResult GetProject(int jaar, string naam)
        {

            Project p = mgr.getProject(jaar, naam);
            List<string> afb = new List<string>();

            if(p == null)
                return StatusCode(HttpStatusCode.NoContent);
            if (p.afbeeldingen != null)
            {
                
                foreach (var item in p.afbeeldingen)
                {
                    char[] chars = new char[item.Afbeelding.Length / sizeof(char)];
                    System.Buffer.BlockCopy(item.Afbeelding, 0, chars, 0, item.Afbeelding.Length);
                    afb.Add(new string(chars));
                }
            }

            DTOProject dp = new DTOProject()
            {
                id= p.Id,
                projectScenario = (int)p.projectScenario,
                titel = p.titel,
                vraag = p.vraag,
                extraInfo = p.extraInfo,
                bedrag = p.bedrag,
                minBedrag = p.minBedrag,
                maxBedrag = p.maxBedrag,
                boekjaar = jaar,
                gemeente = naam,
                cats = convertInspraakItems(p.inspraakItems),
                afbeeldingen = afb
            };
            return Ok(dp);
        }

        [HttpGet]
        public IHttpActionResult GetProjects( string naam)
        {
            List<Project> p = mgr.getProjects( naam).ToList();

            if (p == null || p.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOProject> dp = new List<DTOProject>();

            //elk project
            foreach (var item in p)
            {
                List<DTOBegrotingVoorstel> lbv = new List<DTOBegrotingVoorstel>();
                if (item.voorstellen != null)
                {
                   
                    //voor elke voorstellen in project
                    foreach (var voorstel in item.voorstellen)
                    {

                        DTOBegrotingVoorstel bv = new DTOBegrotingVoorstel()
                        {
                            Id = voorstel.Id,
                            beschrijving = voorstel.beschrijving,
                            samenvatting = voorstel.samenvatting,
                            budgetWijzigingen = new List<DTOBudgetWijziging>()
                        };

                        
                        //voor elke wijziging in voorstel
                        foreach (var wijziging in voorstel.budgetWijzigingen)
                        {
                            DTOBudgetWijziging dw = new DTOBudgetWijziging()
                            {
                                bedrag = wijziging.bedrag,
                                beschrijving = wijziging.beschrijving,
                            };
                            var gemCat = wijziging.inspraakItem as GemeenteCategorie;
                            var actie = wijziging.inspraakItem as Actie;
                            if (gemCat != null)
                                dw.InspraakItem = gemCat.categorieNaam;
                            else
                                dw.InspraakItem = actie.actieKort;

                            bv.budgetWijzigingen.Add(dw); 
                        }

                        lbv.Add(bv);
                    }

                }

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
                    gemeente = naam,
                    voorstellen = lbv
             
                });

            }
            return Ok(dp);
        }

        [Route("postVoorstel/{id}")]
        [HttpPost]
        public IHttpActionResult Post(int id, DTOBegrotingVoorstel p)
        {
            //bedrag, beschrijving, idInspraakItem
            List<Tuple<float, string, int>> bugetwijzigingen = new List<Tuple<float, string, int>>();

            if(p.budgetWijzigingen != null)
            {
                foreach (var item in p.budgetWijzigingen)
                {
                    bugetwijzigingen.Add(new Tuple<float, string, int>(item.bedrag,item.beschrijving, item.inspraakItemId));
                }
                
            }
            mgr.addBegrotingsVoorstel(id, p.auteurEmail, p.beschrijving, p.samenvatting,
                p.totaal, bugetwijzigingen);
            return Ok();
        }
        [Route("putVoorstel/{id}")]
        [HttpPut]
        public IHttpActionResult put(int id, [FromBody]int status)
        {
            mgr.changeVoorstel(id, status);
            return Ok();
        }



        private List<DTOGemeenteCategorie> convertInspraakItems(IEnumerable<InspraakItem> lijnen)
        {
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
                        naamCat = gemCat.categorieNaam,
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
            return gemcats;
        }

    }
}


