using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Cluster
    {
        public int clusterId { get; set; }
        public string name { get; set; }

        public Cluster() { }

        public Cluster(string name) { this.name = name; }

        public override string ToString()
        {
            return name.ToString();
        }


    }






}
