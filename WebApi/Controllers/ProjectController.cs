﻿using BL;
using BL.Domain;
<<<<<<< HEAD
=======
using BL.Domain.DTOs;
>>>>>>> 51d21a0e085018b6f294792f0f8e5db245266cf4
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
<<<<<<< HEAD
using WebApi.Models.DTOs;
=======
using WebApi.Models.DTO;
>>>>>>> 51d21a0e085018b6f294792f0f8e5db245266cf4

namespace WebApi.Controllers
{
    public class ProjectController : ApiController
    {
        private ProjectManager mgr = new ProjectManager();
<<<<<<< HEAD
        private BegrotingManager begMgr = new BegrotingManager();

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
                    gemcats.Add(new DTOGemeenteCategorie() {

                        ID = gemCat.ID,
                        totaal = gemCat.totaal,
                        naamCatz = gemCat.categorieNaam,
                        inspraakNiveau = (int?)gemCat.inspraakNiveau,
                        gemcatID = gemCat.parentGemCatId,
                    });

                }
            }
            foreach (var item in lijnen)
            {
                var actie = item as Actie;
                if(actie != null)
                {
                    var gemcat = gemcats.Where(x => x.ID == item.parentGemCatId).SingleOrDefault();

                    if(gemcat.acties == null)
                    {
                        gemcat.acties = new List<DTOActie>();
                    }
                    gemcat.acties.Add(new DTOActie()
                    {
                         actieKort = actie.actieKort,
                         actieLang = actie.actieLang,
                         inspraakNiveau = (int)actie.inspraakNiveau,
                         uitgaven = actie.uitgaven,
                         ID = actie.ID
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

                if(item.acties != null)
                {
                    foreach (var actie in item.acties)
                    {
                        inspraakItems.Add(new KeyValuePair<int, int>(actie.ID, actie.inspraakNiveau));
                    }
                }
                
            }

            mgr.addProject((ProjectScenario)p.projectScenario, p.titel, p.vraag, p.extraInfo, p.bedrag,
              p.minBedrag, p.minBedrag, inspraakItems, p.boekjaar, p.gemeente);
            return Ok();
        }
=======
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



>>>>>>> 51d21a0e085018b6f294792f0f8e5db245266cf4

    }




}

    
    