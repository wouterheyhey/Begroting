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
    public class BegrotingController : ApiController
    {
        private BegrotingManager begMgr = new BegrotingManager();
        private FinancieleLijnManager finMgr = new FinancieleLijnManager();

        public IHttpActionResult Get(int jaar, string naam)
        {
            IEnumerable<GemeenteCategorie> lijnen = begMgr.readGemeenteCategories(jaar, naam);

            if (lijnen == null || lijnen.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            List<DTOGemeenteCategorie> DTOgemcats = new List<DTOGemeenteCategorie>();

            foreach (var item in lijnen)
            {
                DTOGemeenteCategorie d = new DTOGemeenteCategorie()
                {
                    ID = item.ID,
                    totaal = item.totaal,
                    naamCatz = item.categorieNaam
                };

                if (item.parentGemCat != null)
                {
                    d.naamCaty = item.parentGemCat.categorieNaam;
                    if (item.parentGemCat.parentGemCat != null)
                    {
                        d.naamCatx = item.parentGemCat.parentGemCat.categorieNaam;
                    }
                }


                DTOgemcats.Add(d);
            }

            return Ok(DTOgemcats);
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
                    inspraakNiveau = (int)item.inspraakNiveau
                });
            }

            return Ok(DTOacties);
        }

        //Deze heb ik even aangepast omdat we nu 2 get hebben met zelfde paramaters
        public IHttpActionResult Get()
        {
            try
            {
                //whe 11/5 niet meer nodig??
                //catMgr.SetChildrenCategorien();
                finMgr.LoadFinancieleLijnen(2020);
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
