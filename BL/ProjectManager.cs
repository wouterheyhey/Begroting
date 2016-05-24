using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;
using BL.Domain;

namespace BL
{
   public class ProjectManager
    {
        private ProjectRepository repo;

        public ProjectManager()
        {
            repo = new ProjectRepository();
        }

        public ProjectManager(UnitOfWorkManager uofMgr)
        {
            repo = new ProjectRepository(uofMgr.UnitOfWork);
        }

        public IEnumerable<InspraakItem> getInspraakItems(int jaar, string gemeente)
        {
            return repo.getInspraakItems(jaar, gemeente);
        }

        public int addProject(ProjectScenario ps, string tit, string vra, string info, float bedrag, float min, float max, IDictionary<int, int> inspraakItems, int? boekjaar, string gemeente, bool isActief, string afbeelding)
        {
            Project p = new Project()
            {
                projectScenario = ps,
                titel = tit,
                vraag = vra,
                extraInfo = info,
                bedrag = bedrag,
                minBedrag = min,
                maxBedrag = max,
                isActief = isActief

            };

           return  repo.createProject(p, inspraakItems, afbeelding ,boekjaar, gemeente);
        }

        public int changeProject(int id, ProjectScenario projectScenario, string titel, string vraag, string extraInfo, float bedrag, float minBedrag, float maxBedrag, IDictionary<int, int> inspraakItems, int? boekjaar, string gemeente, bool isActief, string afbeelding)
        {
            return repo.updateProject(id, projectScenario, titel, vraag, extraInfo, bedrag, minBedrag, maxBedrag, inspraakItems, boekjaar, gemeente, isActief, afbeelding);
        }

        public Project getProject(int jaar, string gemeente)
        {
            return repo.readProject(jaar, gemeente);
        }

        public IEnumerable<Project> getProjects( string gemeente)
        {
            return repo.readProjects( gemeente);
        }

        public void addBegrotingsVoorstel(int id, string auteurEmail, string beschrijving, string samenvatting, 
            float totaal, List<Tuple<float, string, int>> budgetwijzigingen, List<string> afbeeldingen)
        {
           
            BegrotingsVoorstel b = new BegrotingsVoorstel()
            {
                beschrijving  = beschrijving,
                samenvatting = samenvatting,
                totaal = totaal,
                indiening = DateTime.Now,
                verificatieStatus = VerificatieStatus.teBehandelen
            };
            
            
            repo.createBegrotingsVoorstel(id, b, auteurEmail, budgetwijzigingen, afbeeldingen);
        }

        public void changeVoorstel(int id, int status)
        {
            repo.UpdateVoorstel( id, status);
        }

        public int changeAantalStemmenVoorstel(int id, string email)
        {
           
           return repo.updateAantalStemmenVoorstel(id, email);
        }

        public int addReactieVoorstel(int id, string email, string reactie)
        {
            return repo.createReactieVoorstel(id, email, reactie);
        }
    }
}
