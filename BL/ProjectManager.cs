using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.repositories;
namespace BL
{
   public class ProjectManager
    {
        ProjectRepository repo;

        public ProjectManager()
        {
            repo = new ProjectRepository();
        }
    }
}
