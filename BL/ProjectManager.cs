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
    }
}
