using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.repositories
{
    public class BegrotingRepository
    {
        private BegrotingDBContext ctx;

        public BegrotingRepository()
        {
            ctx = new BegrotingDBContext();
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        public IEnumerable<Actie> GetActies(int id)
        {
            return ctx.Acties.Where(a => a.parentGemCatId == id).Distinct();
        }
        public IEnumerable<FinancieelOverzicht> getBegrotingen()
        {
            return from g in  ctx.FinancieleOverzichten select g;
            //.OfType<GemeenteCategorie>().Select(z=> z.categorieNaam)
        }

        public IEnumerable<GemeenteCategorie> getGemeenteCategories(int jaar, string naam)
        {
            var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == naam)
               .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
               .Select(c => c.Id).SingleOrDefault();


            // zo ophalen omdat dit multilevel recurieve objecten zijn
            var gemeentecats = from g in ctx.GemeenteCategorien where g.financieelOverzicht.Id == id select g;

            return gemeentecats;
        }

        public IEnumerable<Actie> ReadActies()
        {
            return ctx.Acties;
        }


        public Actie CreateActie(Actie actie)
        {
            ctx.Acties.Add(actie);
            ctx.SaveChanges();
            return actie;
        }

        public Actie CreateNoSaveActie(Actie actie)
        {
            ctx.Acties.Add(actie);
            return actie;
        }

        public void UpdateActie(Actie a)
        {
            ctx.Entry(a).State = EntityState.Modified;
            ctx.SaveChanges();
        }


        public Actie CreateIfNotExistsActie(string actieKort, string actieLang, List<Actie> acties, BestuurType bt, FinancieelOverzicht fo, int gemCatID)
        {

            //nakijken of het om dezelfde actie gaat zoja optellen van inkomsten en uitgaven anders nieuwe actie maken
            
            GemeenteCategorie gemC = ctx.GemeenteCategorien.Find(gemCatID);


            Actie actie = acties.Find(x => x.financieelOverzicht.Id == fo.Id && x.actieKort == actieKort && x.actieLang == actieLang
              && x.bestuurType == bt && x.parentGemCat.ID == gemCatID);
            if (actie == null)
            {
                ctx.Entry(fo).State = EntityState.Unchanged;
                // Actie creeeren zonder inkomsten en uitgaven aangezien deze later aangevuld worden
                return CreateActie(new Actie(actieKort, actieLang, bt, fo, gemC));
            }

            return actie;

        }

        internal void UpdateActieCumulative(Actie actie, float inkomsten, float uitgaven)
        {
            actie.uitgaven += uitgaven;
            actie.inkomsten += inkomsten;
            actie.totaal += actie.calculateTotal(inkomsten, uitgaven); // logica naar manager?
            UpdateActie(actie);
            return;
        }




        public FinancieelOverzicht ReadFinancieelOverzicht(int jaar, HoofdGemeente gemeente)
        {
            return ctx.FinancieleOverzichten.Where<FinancieelOverzicht>(x => x.boekJaar == jaar).Where<FinancieelOverzicht>(x => x.gemeente.naam == gemeente.naam).SingleOrDefault();
        }

        public IEnumerable<FinancieelOverzicht> ReadFinancieelOverzichten()
        {
            return ctx.FinancieleOverzichten;
        }

        public JaarBegroting CreateFinancieelOverzicht(JaarBegroting jaarBegroting)
        {
            ctx.FinancieleOverzichten.Add(jaarBegroting);
            ctx.SaveChanges();
            return jaarBegroting;
        }

        public JaarRekening CreateFinancieelOverzicht(JaarRekening jaarRekening)
        {
            ctx.FinancieleOverzichten.Add(jaarRekening);
            ctx.SaveChanges();
            return jaarRekening;
        }

        public JaarBegroting CreateNoSaveFinancieelOverzicht(JaarBegroting jaarBegroting)
        {
            ctx.FinancieleOverzichten.Add(jaarBegroting);
            return jaarBegroting;
        }

        public JaarRekening CreateNoSaveFinancieelOverzicht(JaarRekening jaarRekening)
        {
            ctx.FinancieleOverzichten.Add(jaarRekening);
            return jaarRekening;
        }

        public FinancieelOverzicht CreateIfNotExistsFinancieelOverzicht(int jaar, HoofdGemeente gem, List<FinancieelOverzicht> fos)
        {
            List<FinancieelOverzicht> fosSubList = fos.FindAll(x => !HoofdGemeente.Equals(x.gemeente, null));
            FinancieelOverzicht fo = fosSubList.Find(x => x.boekJaar.Equals(jaar) && x.gemeente.HoofdGemeenteID.Equals(gem.HoofdGemeenteID));
            if (fo == null)
            {
                ctx.Entry(gem).State = EntityState.Unchanged;
                switch (CreateRekeningNotBegroting(jaar))
                {
                    case true:
                        fo = CreateFinancieelOverzicht(new JaarRekening(jaar, gem));
                        break;
                    case false:
                        fo = CreateFinancieelOverzicht(new JaarBegroting(jaar, gem));
                        break;
                }
            }
            return fo;
        }

        // logic to decide if begroting or rekening. naar manager/domain?
        private bool CreateRekeningNotBegroting(int jaar)
        {     
            // als het jaar huidig of verleden is, gaat het om een rekening, geen begroting
            return  jaar <= DateTime.Now.Year;
        }

       




    }
}
