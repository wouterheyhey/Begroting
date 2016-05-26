using BL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.repositories
{
    public class ProjectRepository
    {
        private BegrotingDBContext ctx;

        public ProjectRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        // Constructor for Unit of Work
        public ProjectRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        public int createProject(Project p, IDictionary<int, int> inspraakItems, string afbeelding, int? boekjaar, string gemeente, string beheerder)
        {
            //we gaan ervan uit dat er  maar 1 project mag zijn per begroting
            Project pp = readProject((int)boekjaar, gemeente);
            Gebruiker g = ctx.Gebruikers.Where(x => x.email == beheerder).SingleOrDefault();
            if (pp == null)
            {
                p.inspraakItems = new HashSet<InspraakItem>();

                if (inspraakItems != null)
                {
                    //ID = key  &&  value = InspraakNiveau
                    foreach (var item in inspraakItems)
                    {
                        InspraakItem i = updateInspraakItem(item.Key, item.Value);
                        if (i != null)
                        {

                            p.inspraakItems.Add(i);
                        }

                    }
                }

                if (afbeelding != null)
                {
                  
                        byte[] bytes = new byte[afbeelding.Length * sizeof(char)];
                        System.Buffer.BlockCopy(afbeelding.ToCharArray(), 0, bytes, 0, bytes.Length);
                    p.afbeelding = bytes;

                }

                FinancieelOverzicht fov = ctx.FinancieleOverzichten.Where(fo => fo.gemeente.naam == gemeente && fo.boekJaar == boekjaar).SingleOrDefault();

                    p.begroting = (JaarBegroting)fov;
                    p.beheerder = g;
                    ctx.Projecten.Add(p);
                    ctx.SaveChanges();
                // return p.Id;     // wordt 0 met unit of work aangezien de save wordt uitgesteld
                return 0;
            }
            else return 1; //om aan te geven dat de create niet gelukt is aangezien we niet met id kunnen werken
            
        }

        public int updateProject(int id, ProjectScenario ps, string tit, string vra, string info, float bedrag, float min, float max, IDictionary<int, int> inspraakItems, int? boekjaar, string gemeente, bool isActief, string afbeelding)
        {
            Project pp = ctx.Projecten.Include(i => i.inspraakItems).Where( p => p.Id ==id).SingleOrDefault();
            if (pp != null)
            {
                if (afbeelding != null)
                {

                    byte[] bytes = new byte[afbeelding.Length * sizeof(char)];
                    System.Buffer.BlockCopy(afbeelding.ToCharArray(), 0, bytes, 0, bytes.Length);
                   // pp.afbeelding = bytes;
                   string convert = afbeelding.Replace("data:image/png;base64,", "");
                    convert = convert.Replace("data:image/jpeg;base64,", "");
                    pp.afbeelding = Convert.FromBase64String(convert);

                }
                if (inspraakItems != null)
                {
                    //ID = key  &&  value = InspraakNiveau
                    foreach (var item in inspraakItems)
                    {
                        InspraakItem i = updateInspraakItem(item.Key, item.Value);
                    }

                }

                pp.bedrag = bedrag;
                pp.maxBedrag = max;
                pp.minBedrag = min;
                pp.projectScenario = ps;
                pp.titel = tit;
                pp.vraag = vra;
                pp.isActief = isActief;

                ctx.Entry(pp).State = EntityState.Modified;
                ctx.SaveChanges();
                //  return pp.Id;  // zal 0 zijn wanneer unit of work gebruikt wordt
                return 0;
            }
            else
                return 1; //om aan te geven dat de update niet gelukt is aangezien we niet met id kunnen werken
        }

        public int updateAantalStemmenVoorstel(int id, string email)
        {
            BegrotingsVoorstel v = ctx.Voorstellen.Include(s => s.stemmen.Select(g => g.gebruiker)).Where(v1 => v1.Id == id).SingleOrDefault();

            if (v != null && v.stemmen.Any(x => x.gebruiker.email == email) == false)
            {
 
                Stem s = new Stem()
                {
                    registratieDatum = DateTime.Now,
                    gebruiker = ctx.Gebruikers.Where(x => x.email == email).SingleOrDefault()

            };
                v.aantalStemmen += 1;
                if (v.stemmen == null)
                {
                    v.stemmen = new HashSet<Stem>();
                }
                v.stemmen.Add(s);
                ctx.Entry(v).State = EntityState.Modified;
                ctx.SaveChanges();
                return v.Id;
            }
            else
                return 0;
        }

        public int createReactieVoorstel(int id, string email, string reactie)
        {
            BegrotingsVoorstel v = ctx.Voorstellen.Include(s => s.reacties.Select(g => g.auteur)).Where(v1 => v1.Id == id).SingleOrDefault();
            if (v != null)
            {

                BegrotingsVoorstelReactie re = new BegrotingsVoorstelReactie()
                {
                    reactieDatum = DateTime.Now,
                    beschrijving = reactie,
                    auteur = ctx.Gebruikers.Where(x => x.email == email).SingleOrDefault()
                };

                if (v.reacties == null)
                {
                    v.reacties = new HashSet<BegrotingsVoorstelReactie>();
                }
                v.reacties.Add(re);
                ctx.Entry(v).State = EntityState.Modified;
                ctx.SaveChanges();
                return v.Id;
            }
            else
                return 0;
        }



        public void UpdateVoorstel(int id, int status, string verificatorEmail)
        {
            BegrotingsVoorstel bv = ctx.Voorstellen.Find(id);
            bv.verificatieStatus = (VerificatieStatus) status;
            bv.verificatieDatum = DateTime.Now;
            bv.verificator = ctx.Gebruikers.Where(x => x.email == verificatorEmail).SingleOrDefault();
            ctx.SaveChanges();
        }

        public void createBegrotingsVoorstel(int id, BegrotingsVoorstel b, string auteurEmail, List<Tuple<float, string, int>> budgetwijzigingen, List<string> afbeeldingen)
        {
            //budgetWijzigingen aanmaken
            if (budgetwijzigingen != null)
            {
                b.budgetWijzigingen = new HashSet<BudgetWijziging>();
                foreach (var item in budgetwijzigingen)
                {
                    b.budgetWijzigingen.Add(createBudgetWijziging(item.Item1, item.Item2, item.Item3));
                }
            }

           

            if(afbeeldingen != null)
            {
                HashSet<VoorstelAfbeelding> vafbeeldingen = new HashSet<VoorstelAfbeelding>();

                foreach (var afb in afbeeldingen)
                {
                    byte[] bytes = new byte[afb.Length * sizeof(char)];
                    System.Buffer.BlockCopy(afb.ToCharArray(), 0, bytes, 0, bytes.Length);

                    vafbeeldingen.Add(new VoorstelAfbeelding(bytes));
                }
                b.afbeeldingen = vafbeeldingen;
            }
            ctx.Voorstellen.Add(b);

            //begrotingsvoorstel toevoegen aan project
            Project p = ctx.Projecten.Include(nameof(Project.voorstellen)).Where(x => x.Id == id).SingleOrDefault();
            p.voorstellen.Add(b);

            //auteurEmail  kan niet null of fout zijn --> Authorized webapi via token
            //aangezien je enkel een voorstel kan indienen als je ingelogd bent met een bestaand email
            b.auteur = ctx.Gebruikers.Where(x => x.email == auteurEmail).SingleOrDefault();

            ctx.SaveChanges();

        }


        public InspraakItem updateInspraakItem(int id, int inspraakNiveau)
        {
            InspraakItem ii = ctx.inspraakItems.Find(id);
            ii.inspraakNiveau = (InspraakNiveau)inspraakNiveau;
            return ii;

        }

        public IEnumerable<InspraakItem> getInspraakItems(int jaar, string naam)
        {
           return ctx.inspraakItems.Where(x => x.financieelOverzicht.boekJaar == jaar && x.financieelOverzicht.gemeente.naam == naam);
        }

        public Project readProject(int jaar, string gemeente)
        {
            return ctx.Projecten.Include(nameof(Project.inspraakItems)).Include(x => x.beheerder).Where(p => p.begroting.gemeente.naam == gemeente 
            && p.begroting.boekJaar == jaar).SingleOrDefault();
        }

        public IEnumerable<Project> readProjects(string gemeente)
        {
             return ctx.Projecten.Include(nameof(Project.begroting)).Include(v => v.voorstellen.Select(w => w.budgetWijzigingen.Select(i => i.inspraakItem)))
                .Include(v => v.voorstellen.Select(r => r.reacties.Select(g => g.auteur)))
                .Where(p => p.begroting.gemeente.naam == gemeente);
            
        }

        public BudgetWijziging createBudgetWijziging(float bedrag, string beschrijving, int id)
        {
            InspraakItem i = ctx.inspraakItems.Find(id);
            BudgetWijziging bw = new BudgetWijziging()
            {
                bedrag = bedrag,
                beschrijving = beschrijving,
                inspraakItem = i
            };
            return bw;
        }

         

        public void saveContext()
        {
            ctx.SaveChanges();
        }
    }
}
