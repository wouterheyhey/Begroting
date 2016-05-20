﻿using BL.Domain;
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

        public int createProject(Project p, IDictionary<int, int> inspraakItems, List<string> afbeeldingen, int? boekjaar, string gemeente)
        {
            //momenteel mag er maar 1 project zijn per begroting
            Project pp = readProject((int)boekjaar, gemeente);
            //momenteel mag er maar 1 project zijn per begroting
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

                if (afbeeldingen != null)
                {
                    p.afbeeldingen = new HashSet<ProjectAfbeelding>();
                    foreach (var item in afbeeldingen)
                    {
                        byte[] bytes = new byte[item.Length * sizeof(char)];
                        System.Buffer.BlockCopy(item.ToCharArray(), 0, bytes, 0, bytes.Length);
                        ProjectAfbeelding a = new ProjectAfbeelding(bytes);
                        p.afbeeldingen.Add(a);
                    }

                }

                FinancieelOverzicht fov = ctx.FinancieleOverzichten.Where(fo => fo.gemeente.naam == gemeente && fo.boekJaar == boekjaar).SingleOrDefault();

                    p.begroting = (JaarBegroting)fov;
                    ctx.Projecten.Add(p);
                    ctx.SaveChanges();
                    return p.Id;     
                
            }
            else return 0;
            
        }

        public void createBegrotingsVoorstel(int id, BegrotingsVoorstel b, string auteurEmail, List<Tuple<float, string, int>> budgetwijzigingen)
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
            ctx.Voorstellen.Add(b);
            ctx.SaveChanges();
            //auteurEmail komt uit token dus kan niet null of fout zijn
            //aangezien je enkel een voorstel kan indienen als je ingelogd bent met een bestaand email

            /* Gebruiker g = ctx.Gebruikers.Find(auteurEmail);
             b.auteur = g; */

            //begrotingsvoorstel toevoegen aan project

            Project p = ctx.Projecten.Include(nameof(Project.voorstellen)).Where(x => x.Id ==id).SingleOrDefault();


            p.voorstellen.Add(b);
            ctx.Entry(p).State = EntityState.Modified;
            ctx.SaveChanges();

        }

        private void updateProject(Project p, BegrotingsVoorstel b)
        {
            
        }

        public InspraakItem updateInspraakItem(int id, int inspraakNiveau)
        {
            InspraakItem ii = ctx.inspraakItems.Find(id);


            ii.inspraakNiveau = (InspraakNiveau)inspraakNiveau;

            return ii;

        }

        public IEnumerable<InspraakItem> getInspraakItems(int jaar, string naam)
        {
            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == naam)
               .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
               .Select(c => c.Id).SingleOrDefault();


            //moeten het zo ophalen omdat dit multilevel recurieve objecten zijn
            var ip = from g in ctx.inspraakItems where g.financieelOverzicht.Id == id select g;

            return ip;
        }

        public Project readProject(int jaar, string gemeente)
        {
            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == gemeente)
               .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
               .Select(c => c.Id).SingleOrDefault();

            return ctx.Projecten.Include(nameof(Project.inspraakItems)).Include(nameof(Project.afbeeldingen)).Where(p => p.begroting.Id == id).SingleOrDefault();
        }

        public IEnumerable<Project> readProjects(string gemeente)
        {
             return ctx.Projecten.Include(nameof(Project.begroting)).Include(v => v.voorstellen.Select(w => w.budgetWijzigingen.Select(i => i.inspraakItem))).Where(p => p.begroting.gemeente.naam == gemeente);
            //return  from g in ctx.Projecten where g.begroting.gemeente.naam == gemeente select g;
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
