using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;

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

        public BegrotingRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
            ctx.Database.Initialize(false);
            ctx.Database.Log = msg => System.Diagnostics.Debug.WriteLine(msg);
        }

        public IEnumerable<Actie> GetActies(int id)
        {
            return ctx.Acties.Where(a => a.parentGemCatId == id).Distinct();
        }
        public IEnumerable<JaarBegroting> getBegrotingen(string naam)
        {
            return ctx.FinancieleOverzichten.Include(nameof(FinancieelOverzicht.gemeente)).Include(nameof(FinancieelOverzicht.lijnen)).Where(e => e.lijnen.Any(p => p is GemeenteCategorie)).Where(y => y.gemeente.naam == naam).OfType<JaarBegroting>();
        }


        public void updateGemcatInput(List<Tuple<int, string, string, string, string, string>> categorieInput)
        {
            foreach (var catinput in categorieInput)
            {
                //Tuple structuur item1 =  gemcatId,item2= string input, item3 = string icoon,item4 = string film,item5 = string foto,item6 = string kleur
                GemeenteCategorie gc = ctx.GemeenteCategorien.Include(x => x.categorieInput).Where(y => y.ID == catinput.Item1).SingleOrDefault();

                if (gc.categorieInput == null)
                {
                    gc.categorieInput = new CategorieInput(catinput.Item2, catinput.Item3, catinput.Item4, catinput.Item5, catinput.Item6);
                }
                else
                {
                    gc.categorieInput.input = catinput.Item2;
                    gc.categorieInput.kleur = catinput.Item6;

                    if (catinput.Item3 != null)
                        gc.categorieInput.icoon = stringConverter(catinput.Item3);

                    if (catinput.Item4 != null)
                        gc.categorieInput.film = stringConverter(catinput.Item4);

                    if (catinput.Item5 != null)
                        gc.categorieInput.foto = stringConverter(catinput.Item5);
                }

                ctx.Entry(gc).State = EntityState.Modified;
            }
            ctx.SaveChanges();
        }



        public IEnumerable<GemeenteCategorie> getGemeenteCategories(int jaar, string naam)
        {
            var gemeentecats = ctx.GemeenteCategorien.Include(x => x.categorieInput).Where(x => x.financieelOverzicht.boekJaar == jaar && x.financieelOverzicht.gemeente.naam == naam);
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
                actie = CreateActie(new Actie(actieKort, actieLang, bt, fo, gemC));
            }
            return actie ;

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

        private byte[] stringConverter(string beeld)
        {
            byte[] bytes = new byte[beeld.Length * sizeof(char)];
            System.Buffer.BlockCopy(beeld.ToCharArray(), 0, bytes, 0, bytes.Length);
             return bytes;
        }                            

    }
}
