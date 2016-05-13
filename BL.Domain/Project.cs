using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Project
    {
        public int Id { get; set; }
        ProjectScenario projectScenario { get; set; }
        string titel { get; set; }
        string vraag { get; set; }
        public string extraInfo { get; set; }
        public float bedrag { get; set; }
        public DateRange dateRange { get; set; } 
        public float minBedrag { get; set; }
        public float maxBedrag { get; set; }
        public string[] toegestanePostcodes { get; set; }
        public HashSet<InspraakItem> inspraakItems { get; set; }
        public HashSet<ProjectAfbeelding> afbeeldingen { get; set; }
        public IngelogdeGebruiker beheerder { get; set; }
        public HashSet<BegrotingsVoorstel> voorstellen { get; set; }

        public JaarBegroting begroting { get; set; }

    }
}
