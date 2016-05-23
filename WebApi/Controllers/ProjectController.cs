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

        [Route("itemsGET")]
        [HttpGet]
        public IHttpActionResult Get(int jaar, string naam)
        {
            ProjectManager mgr = new ProjectManager();
            List<DTOGemeenteCategorie> lijnen = new List<DTOGemeenteCategorie>();
            IEnumerable<InspraakItem> parents = mgr.getInspraakItems(jaar, naam).Where<InspraakItem>(x => x.parentGemCat == null);

            if (parents == null || parents.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            foreach(InspraakItem item in parents)
            {
                lijnen.Add(convertInspraakItem(item, new List<DTOGemeenteCategorie>())); // via hoogste niveau in hierachie de braches opbouwen
            }
            return Ok(lijnen); 
           
        }

         
        [Route("postProject")]
        [HttpPost]
        public IHttpActionResult Post(DTOProject p)
        {
            // Implementatie van de UoW pattern voor de post methodes
            // Voordelen: minder roundtrips, gebruik van transacties
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            ProjectManager mgr = new ProjectManager(uowMgr);
            //K= id + V= inspraakNiveau
            IDictionary<int, int> inspraakItems = new Dictionary<int, int>();

            //nivA
            if(p.cats != null)
            {
                foreach (var A in p.cats)
                {
                    inspraakItems.Add(new KeyValuePair<int, int>(A.ID, (int)A.inspraakNiveau));

                    if (A.childCats != null)
                    {
                        //nivB
                        foreach (var B in A.childCats)
                        {
                            inspraakItems.Add(new KeyValuePair<int, int>(B.ID, (int)B.inspraakNiveau));

                            //nivC
                            if (B.childCats != null)
                            {
                                foreach (var C in B.childCats)
                                {
                                    inspraakItems.Add(new KeyValuePair<int, int>(C.ID, (int)C.inspraakNiveau));
                                    if (C.acties != null)
                                    {
                                        foreach (var actie in C.acties)
                                        {
                                            inspraakItems.Add(new KeyValuePair<int, int>(actie.ID, actie.inspraakNiveau));
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

           int id =  mgr.addProject((ProjectScenario)p.projectScenario, p.titel, p.vraag, p.extraInfo, p.bedrag,
              p.minBedrag, p.maxBedrag, inspraakItems, p.boekjaar, p.gemeente, p.isActief, p.afbeelding);
            uowMgr.Save();

            return Ok(id);
        }
        [Route("updateProject//{id}")]
        [HttpPost]
        public IHttpActionResult put(int id, DTOProject p)
        {
            //K= id + V= inspraakNiveau
            IDictionary<int, int> inspraakItems = new Dictionary<int, int>();

            //nivA
            if (p.cats != null)
            {
                foreach (var A in p.cats)
                {
                    inspraakItems.Add(new KeyValuePair<int, int>(A.ID, (int)A.inspraakNiveau));

                    if (A.childCats != null)
                    {
                        //nivB
                        foreach (var B in A.childCats)
                        {
                            inspraakItems.Add(new KeyValuePair<int, int>(B.ID, (int)B.inspraakNiveau));

                            //nivC
                            if (B.childCats != null)
                            {
                                foreach (var C in B.childCats)
                                {
                                    inspraakItems.Add(new KeyValuePair<int, int>(C.ID, (int)C.inspraakNiveau));
                                    if (C.acties != null)
                                    {
                                        foreach (var actie in C.acties)
                                        {
                                            inspraakItems.Add(new KeyValuePair<int, int>(actie.ID, actie.inspraakNiveau));
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

            int idP = mgr.changeProject(id, (ProjectScenario)p.projectScenario, p.titel, p.vraag, p.extraInfo, p.bedrag,
               p.minBedrag, p.minBedrag, inspraakItems, p.boekjaar, p.gemeente, p.isActief, p.afbeelding);

            if (idP == 0)
                return BadRequest("Er is iets misgelopen bij het updaten van het project!");
            return Ok(idP);
        }


        [HttpGet]
        public IHttpActionResult GetProjects( string naam)
        {
            ProjectManager mgr = new ProjectManager();
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
                            aantalStemmen = voorstel.aantalStemmen,
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
                        //voor elke reactie op een voorstel
                        foreach (var reactie in voorstel.reacties)
                        {
                            DTOReactie re = new DTOReactie()
                            {
                                //@TODO veranderen naar reactie.auteur.email
                                email = "test@test.be",
                                beschrijving = reactie.beschrijving,
                            };
                            re.reactieDatum = ((DateTime)reactie.reactieDatum).ToString("d");

                            bv.reacties.Add(re);
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

        // id: projectID
        [Route("postVoorstel/{id}")]
        [HttpPost]
        public IHttpActionResult Post(int id, DTOBegrotingVoorstel p)
        {
            // Implementatie van de UoW pattern voor de post methodes
            // Voordelen: minder roundtrips, gebruik van transacties
            UnitOfWorkManager uowMgr = new UnitOfWorkManager();
            ProjectManager mgr = new ProjectManager(uowMgr);

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
            // Commit UoW to the database
            // Previous saves on the context were blocked because of the presence of the UnitOfWork
            uowMgr.Save();
            return Ok();
        }

        [Route("putStem/{id}")]
        [HttpPut]
        public IHttpActionResult put(int id, [FromBody]string email)
        {
            ProjectManager mgr = new ProjectManager();
            int idStem = mgr.changeAantalStemmenVoorstel(id, email);
            return Ok(idStem);
        }

        [Route("postReactie/{id}")]
        [HttpPost]
        public IHttpActionResult postReactie(int id, DTOReactie r)
        {
            ProjectManager mgr = new ProjectManager();
            int idStem = mgr.addReactieVoorstel(id, r.email, r.beschrijving);
            return Ok(idStem);
        }

        [Route("putVoorstel/{id}")]
        [HttpPut]
        public IHttpActionResult put(int id, [FromBody]int status)
        {
            ProjectManager mgr = new ProjectManager();
            mgr.changeVoorstel(id, status);
            return Ok();
        }


        [Route("projectGET")]
        [HttpGet]
        public IHttpActionResult GetProject(int jaar, string naam)
        {
            ProjectManager mgr = new ProjectManager();
            Project p = mgr.getProject(jaar, naam);
            string afb="";

            if (p == null)
                return StatusCode(HttpStatusCode.NoContent);
            if (p.afbeelding != null)
            {

                 char[] chars = new char[p.afbeelding.Length / sizeof(char)];
                  afb = new string(chars);
               
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
                afbeelding = afb
            };
            foreach (GemeenteCategorie gemCat in p.inspraakItems.Where<InspraakItem>(x => x.parentGemCat == null))
            {
                dp.cats.Add(convertInspraakItem(gemCat, new List<DTOGemeenteCategorie>()));
            }
            return Ok(dp);
        }

        private DTOGemeenteCategorie convertInspraakItem(InspraakItem item, List<DTOGemeenteCategorie> gemCats)

        {
            CategorieManager catMgr = new CategorieManager();
            var gemCat = item as GemeenteCategorie;

            if (gemCat != null)
            {
                DTOGemeenteCategorie d = MapGemCatToDTOGemCat(gemCat);
                gemCats.Add(d);

                foreach (InspraakItem ii in catMgr.GetChildrenInspraakItem(item))
                {
                    DTOGemeenteCategorie dtoGemCat = convertInspraakItem(ii, gemCats);  // recursie om lagere niveaus op te halen
                    if (dtoGemCat.ID != 0)
                    {
                        d.childCats.Add(dtoGemCat);
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


