using BL;
using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Models.DTO;

namespace WebApi.Controllers
{
    [RoutePrefix("api/begroting")]
    public class BegrotingController : ApiController
    {
        private BegrotingManager begMgr = new BegrotingManager();
        private FinancieleLijnManager finMgr = new FinancieleLijnManager();
        private CategorieManager catMgr = new CategorieManager();

        public IHttpActionResult Get(int jaar, string naam)
        {
            IEnumerable<GemeenteCategorie> gemCats = begMgr.readGemeenteCategories(jaar, naam);

            if (gemCats == null || gemCats.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOGemeenteCategorie> DTOgemcats = new List<DTOGemeenteCategorie>();

            List<InspraakItem> parents;
            foreach (var item in gemCats)
            {
                parents = new List<InspraakItem>();
                catMgr.GetAllParents(item,parents);
                DTOgemcats.Add(MapGemCatToDTOGemCatWithParents(item,parents));
            }         

            return Ok(DTOgemcats);

        }
   
        private DTOGemeenteCategorie MapGemCatToDTOGemCatWithParents(GemeenteCategorie gemCat, List<InspraakItem> gemCats)
        {
            DTOGemeenteCategorie d = new DTOGemeenteCategorie();

            foreach (InspraakItem item in gemCats.Where(x=>x is GemeenteCategorie))
                {
                // platmaken van de hierarchische structuur met het oog op de graphs
                // elke categorieType komt maximaal 1x voor als je de parents meegeeft 
                d = MapTypeToPropery(((GemeenteCategorie)item), d);
                 //   d.cats[((GemeenteCategorie)item).categorieType.ToString()] = ((GemeenteCategorie)item).categorieNaam;
                }
            d.ID = gemCat.ID;
            d.totaal = gemCat.totaal;
            // d.cats[gemCat.categorieType.ToString()] = gemCat.categorieNaam;

            d = MapTypeToPropery(gemCat, d);

            return d;
        }

        private DTOGemeenteCategorie MapTypeToPropery(GemeenteCategorie item, DTOGemeenteCategorie d)
        {
            switch (item.categorieType.ToString())
            {
                case ("A"):
                    d.catA = item.categorieNaam;
                    break;
                case ("B"):
                    d.catB = item.categorieNaam;
                    break;
                case ("C"):
                    d.catC = item.categorieNaam;
                    break;
            }
            return d;
        }
    

        public IHttpActionResult Get(int id)
        {
            IEnumerable<Actie> acties = begMgr.readActies(id);
            if (acties == null || acties.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOActie> DTOacties = new List<DTOActie>();
            foreach (var item in acties)
            {
                DTOacties.Add(new DTOActie()
                {
                    ID = item.ID,
                    actieKort = item.actieKort,
                    actieLang = item.actieLang,
                    uitgaven = item.uitgaven,
                    inspraakNiveau = (int)item.inspraakNiveau,
                    bestuurtype = (int)item.bestuurType
                });
            }

            return Ok(DTOacties);
        }

        [Route("getBegrotingen")]
        [HttpGet]
        public IHttpActionResult getBegrotingen()
        {
            IEnumerable<FinancieelOverzicht> jbs = begMgr.readBegrotingen();
            return Ok(jbs);
        }

        //Deze heb ik even aangepast omdat we nu 2 get hebben met zelfde paramaters
        public IHttpActionResult Get()
        {
            try
            {
                //whe 11/5 niet meer nodig??
                //catMgr.SetChildrenCategorien();
                // 2020 omdat dit relatief weinig data bevat
                finMgr.LoadFinancieleLijnen("gemeente_categorie_acties_jaartal_uitgaven.xlsx", 2020);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.OK);

        }

        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

    }

}