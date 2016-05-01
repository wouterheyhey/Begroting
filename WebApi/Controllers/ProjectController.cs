using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProjectController : ApiController
    {
        private ProjectManager mgr = new ProjectManager();
    }
}
