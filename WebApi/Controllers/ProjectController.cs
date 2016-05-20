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
        public IHttpActionResult GetProjects(string naam)
        {
            IEnumerable<Project> p = mgr.getProjects(naam);

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
                //catMgr.GetHighestLevelCats()
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


