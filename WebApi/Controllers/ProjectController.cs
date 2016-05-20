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
        private CategorieManager catMgr = new CategorieManager();
        [Route("itemsGET")]
        [HttpGet]
        public IHttpActionResult Get(int jaar, string naam)
        {
            IEnumerable<InspraakItem> lijnen = mgr.getInspraakItems(jaar, naam);

            if (lijnen == null || lijnen.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(convertInspraakItem(lijnen.First(), null)); // Moet aangepast worden
           
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
                            verificatieStatus = (int)voorstel.verificatieStatus,
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

        [Route("putReactieEnStem/{id}")]
        [HttpPut]
        public IHttpActionResult put(int id, [FromBody]string email)
        {
            int idStem = mgr.changeAantalStemmenEnReactiesVoorstel(id, email);
           return Ok(idStem);
        }

        [Route("putVoorstel/{id}")]
        [HttpPut]
        public IHttpActionResult put(int id, [FromBody]int status)
        {
            mgr.changeVoorstel(id, status);
            return Ok();
        }


        [Route("projectGET")]
        [HttpGet]
        public IHttpActionResult GetProject(int jaar, string naam)
        {

            Project p = mgr.getProject(jaar, naam);
            List<string> afb = new List<string>();

            if (p == null)
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
                id = p.Id,
                projectScenario = (int)p.projectScenario,
                titel = p.titel,
                vraag = p.vraag,
                extraInfo = p.extraInfo,
                bedrag = p.bedrag,
                minBedrag = p.minBedrag,
                maxBedrag = p.maxBedrag,
                boekjaar = jaar,
                gemeente = naam,
                cats = new List<DTOGemeenteCategorie>(),
                afbeeldingen = afb
            };
            foreach (GemeenteCategorie gemCat in p.inspraakItems.Where<InspraakItem>(x => x.parentGemCat == null))
            {
                dp.cats.Add(convertInspraakItem(gemCat, new List<DTOGemeenteCategorie>()));
            }
            return Ok(dp);
        }

        private DTOGemeenteCategorie convertInspraakItem(InspraakItem item, List<DTOGemeenteCategorie> gemCats)

        {
            var gemCat = item as GemeenteCategorie;

            if (gemCat != null)
            {
                DTOGemeenteCategorie d = MapGemCatToDTOGemCat(gemCat);
                gemCats.Add(d);

                foreach (InspraakItem ii in catMgr.GetChildrenInspraakItem(item))
                {
                    DTOGemeenteCategorie dtoGemCat = convertInspraakItem(ii, gemCats);
                    if (dtoGemCat.ID != 0)
                    {
                        d.childCats.Add(dtoGemCat); // add whole list in once, recursie
                    }
                }
                return d;

            }

            var actie = item as Actie;
            if (actie != null)
            {
                var gemcat = gemCats.Where(x => x.ID == item.parentGemCatId).SingleOrDefault();

                if (gemcat.acties == null)
                {
                    gemcat.acties = new List<DTOActie>();
                }
                gemcat.acties.Add(MapActieToDTOActie(actie));
            }

            return new DTOGemeenteCategorie(); // niets terug sturen. Correcte gemeenteCategorie werd hierboven teruggestuurd
        }

        private DTOActie MapActieToDTOActie(Actie actie)
        {
            return new DTOActie()
            {
                actieKort = actie.actieKort,
                actieLang = actie.actieLang,
                inspraakNiveau = (int)actie.inspraakNiveau,
                uitgaven = actie.uitgaven,
                ID = actie.ID,
                bestuurtype = (int)actie.bestuurType
            };
        }

        private DTOGemeenteCategorie MapGemCatToDTOGemCat(GemeenteCategorie gemCat)
        {
            DTOGemeenteCategorie d = new DTOGemeenteCategorie();
            d.ID = gemCat.ID;
            d.totaal = gemCat.totaal;
            d.naamCat = gemCat.categorieNaam;
            d.inspraakNiveau = (int?)gemCat.inspraakNiveau;
            d.gemcatID = gemCat.parentGemCatId;
            d.catCode = gemCat.categorieCode;
            d.catType = gemCat.categorieType.ToString();
            d.childCats = new List<DTOGemeenteCategorie>();

            return d;
        }

    }
}


