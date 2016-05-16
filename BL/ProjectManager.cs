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
        ProjectRepository repo;

        public ProjectManager()
        {
            repo = new ProjectRepository();
        }

        public IEnumerable<InspraakItem> getInspraakItems(int jaar, string gemeente)
        {
            return repo.getInspraakItems(jaar, gemeente);
        }

        public int addProject(ProjectScenario ps, string tit, string vra, string info, float bedrag, float min, float max, IDictionary<int, int> inspraakItems, int? boekjaar, string gemeente)
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

            };

           return  repo.createProject(p, inspraakItems, boekjaar, gemeente);
        }

        public Project getProject(int jaar, string gemeente)
        {
            return repo.readProject(jaar, gemeente);
        }

        public IEnumerable<Project> getProjects( string gemeente)
        {
            return repo.readProjects( gemeente);
        }

    }
}
