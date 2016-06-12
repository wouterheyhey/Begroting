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
        [AllowAnonymous]
        [Route("getClusterAverages")]
        [HttpGet]
        public IHttpActionResult getClusterAverages(string gemeenteNaam, int jaar)
        {
            GemeenteManager gemMgr = new GemeenteManager();
            CategorieManager catMgr = new CategorieManager();

            List<Categorie> cats = catMgr.ReadClusterAverage(gemMgr.GetCluster(gemeenteNaam), jaar).ToList();
            // adding parents to the newly created categories
            foreach (Categorie cat in cats)
            {
                // parent always null
                cat.categorieParent = catMgr.ReadClosestParent(cat.categorieCode,cat.categorieType);
            }
                
            if (cats == null || cats.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOCategorie> DTOCats = new List<DTOCategorie>();

            List<Categorie> parents;
            foreach (Categorie cat in cats)
            {
                parents = new List<Categorie>();
                catMgr.GetAllParentsCategorien(cat, parents);
                DTOCats.Add(MapCatToDTOCatWithParents(cat, parents));
            }
            if(DTOCats == null || DTOCats.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

            return Ok(DTOCats);
        }



        [AllowAnonymous]
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

 

        private DTOCategorie MapCatToDTOCatWithParents(Categorie cat, List<Categorie> cats)
        {
            DTOCategorie d = new DTOCategorie();

            foreach (Categorie item in cats)
                {
                // platmaken van de hierarchische structuur met het oog op de graphs
                // elke categorieType komt maximaal 1x voor als je de parents meegeeft 
                d = MapTypeToProperyCategorie(item, d);
                 //   d.cats[((GemeenteCategorie)item).categorieType.ToString()] = ((GemeenteCategorie)item).categorieNaam;
                }
            d.ID = cat.ToString().GetHashCode(); 
            d.totaal = cat.totaal;
            d.catCode = cat.categorieCode;
            d.naamCat = cat.categorieNaam;
            // d.cats[gemCat.categorieType.ToString()] = gemCat.categorieNaam;

            d = MapTypeToProperyCategorie(cat, d);

            return d;
        }

        private DTOCategorie MapTypeToProperyCategorie(Categorie item, DTOCategorie d)
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

        private DTOGemeenteCategorie MapGemCatToDTOGemCatWithParents(GemeenteCategorie gemCat, List<InspraakItem> gemCats)
        {
            DTOGemeenteCategorie d = new DTOGemeenteCategorie();

            foreach (InspraakItem item in gemCats.Where(x => x is GemeenteCategorie))
            {
                // platmaken van de hierarchische structuur met het oog op de graphs
                // elke categorieType komt maximaal 1x voor als je de parents meegeeft 
                d = MapTypeToPropery(((GemeenteCategorie)item), d);
                //   d.cats[((GemeenteCategorie)item).categorieType.ToString()] = ((GemeenteCategorie)item).categorieNaam;
            }
            d.ID = gemCat.ID;
            d.totaal = gemCat.totaal;
            d.naamCat = gemCat.categorieNaam;

            if(gemCat.categorieInput != null)
            {
                if (gemCat.categorieInput.foto != null)
                {
                    d.foto = Convert.ToBase64String(gemCat.categorieInput.foto);
                }

                if (gemCat.categorieInput.film != null)
                {
                    d.film = gemCat.categorieInput.film;
                }

                if (gemCat.categorieInput.icoon != null)
                {
                    d.icoon = gemCat.categorieInput.icoon;
                }

                d.inputID = gemCat.categorieInput.Id;
                d.input = gemCat.categorieInput.input;
                d.kleur = gemCat.categorieInput.kleur;
            }

           
            
          

            // d.cats[gemCat.categorieType.ToString()] = gemCat.categorieNaam;

            d = MapTypeToPropery(gemCat, d);

            return d;
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

        [AllowAnonymous]
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
        [AllowAnonymous]
        [Route("getBegrotingen")]
        [HttpGet]
        public IHttpActionResult getBegrotingen(string naam)
        {
            BegrotingManager begMgr = new BegrotingManager();
            ProjectManager prmgr = new ProjectManager();
            IEnumerable<JaarBegroting> jbs = begMgr.readBegrotingen(naam);
            IEnumerable<Project> project = prmgr.getProjects(naam);
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
                Project p = project.Where(x => x.begroting.Id == item.Id).SingleOrDefault();
                if (p == null)
                    b.hasProject = false;
                else
                    b.hasProject = true;
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

        [Route("loadFinOverzicht")]
        [HttpGet]
        public IHttpActionResult getLoadFinOverzicht(int jaar = 2020)
        {
            FinancieleLijnManager finMgr = new FinancieleLijnManager();
            try
            {
                // 2020 omdat dit relatief weinig data bevat
                finMgr.LoadFinancieleLijnen("gemeente_categorie_acties_jaartal_uitgaven.xlsx", jaar);
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.OK);
        }

        [Authorize(Roles = "admin,superadmin")]
        [Route("changeCatInput")]
        [HttpPut]
        public IHttpActionResult Put(List<DTOGemeenteCategorie> gemcats)
        {
            BegrotingManager begMgr = new BegrotingManager();
            List<Tuple<int, string, string, string, string, string>> categorieInput = new List<Tuple<int, string, string, string, string, string>>();

            foreach (var gemcat in gemcats)
            {
                categorieInput.Add(new Tuple<int, string, string, string, string, string>(gemcat.ID, gemcat.input, gemcat.icoon, gemcat.film, gemcat.foto,
                gemcat.kleur));
            }
            begMgr.changeGemcatInput(categorieInput);

            return Ok();
        }



        [Authorize(Roles = "superadmin")]
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
                    var filePath = HttpContext.Current.Server.MapPath(System.IO.Path.Combine("~/App_Data/", System.IO.Path.GetFileName(postedFile.FileName)));
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