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
                };
                if(item.parentGemCat == null)
                {
                    d.catA = item.categorieNaam;
                }
                else
                {
                    if (item.parentGemCat.parentGemCat == null)
                    {
                        d.catA = item.parentGemCat.categorieNaam;
                        d.catB = item.categorieNaam;
                    }
                    else
                    {
                        d.catA = item.parentGemCat.parentGemCat.categorieNaam;
                        d.catB = item.parentGemCat.categorieNaam;
                        d.catC = item.categorieNaam;
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
                    inspraakNiveau = (int)item.inspraakNiveau,
                    bestuurtype = (int)item.bestuurType
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

    }
}
