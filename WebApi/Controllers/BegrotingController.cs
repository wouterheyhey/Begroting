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

        public IHttpActionResult Get(int jaar, string naam)
        {
            CategorieManager catMgr = new CategorieManager();
            BegrotingManager begMgr = new BegrotingManager();
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
            BegrotingManager begMgr = new BegrotingManager();
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
        public IHttpActionResult getBegrotingen(string naam)
        {
            BegrotingManager begMgr = new BegrotingManager();
            IEnumerable<JaarBegroting> jbs = begMgr.readBegrotingen(naam);
            if (jbs == null || jbs.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);
            List<DTOBegroting> begrotingen = new List<DTOBegroting>();
            foreach (var item in jbs)
            {
                DTOBegroting b = new DTOBegroting()
                {
                    boekjaar = item.boekJaar,
                    childCats = new List<DTOGemeenteCategorie>()
                    
                };
                if(item.lijnen != null)
                {
                    foreach (var lijn in item.lijnen)
                    {
                        var gem = lijn as GemeenteCategorie;
                        if(gem != null && gem.categorieType=="A")
                        {
                            DTOGemeenteCategorie gemcat = new DTOGemeenteCategorie()
                            {
                                naamCat = gem.categorieNaam,
                                totaal = gem.totaal
                            };
                            b.childCats.Add(gemcat);
                        }
                        
                    }

                }
                
                begrotingen.Add(b); 
            }
            return Ok(begrotingen);
        }

        public IHttpActionResult Get()
        {
            FinancieleLijnManager finMgr = new FinancieleLijnManager();
            try
            {
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
            string fileName = default(string);
            
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + postedFile.FileName);
                    fileName = postedFile.FileName;
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (result.StatusCode == HttpStatusCode.Created)
                result = Request.CreateResponse(Get(2020, fileName));
            return result;
        }

    }

}