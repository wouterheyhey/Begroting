using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BL.Domain.DTOs;

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





        /*   public FinancieleLijn CreateFinLijn(FinancieleLijn finLijn)
           {
               ctx.Entry(finLijn.cat).State = EntityState.Unchanged;
               ctx.Entry(finLijn.actie).State = EntityState.Unchanged;
               ctx.Entry(finLijn.financieelOverzicht).State = EntityState.Unchanged;
               ctx.FinLijnen.Add(finLijn);
               ctx.SaveChanges();

               return finLijn;
           }

           public FinancieleLijn CreateNoSaveFinLijn(FinancieleLijn finLijn)
           {
               ctx.Entry(finLijn.cat).State = EntityState.Unchanged;
               ctx.Entry(finLijn.actie).State = EntityState.Unchanged;
               ctx.Entry(finLijn.financieelOverzicht).State = EntityState.Unchanged;
               ctx.FinLijnen.Add(finLijn);

               return finLijn;
           }

           public IEnumerable<DTOfinancieleLijn> GetFinancieleLijnen(int jaar, string naam)
           {


               var id = ctx.FinancieleOverzichten.Include(nameof(JaarBegroting.gemeente)).Where(f1 => f1.gemeente.naam == naam)
                   .Where<FinancieelOverzicht>(f2 => f2.boekJaar == jaar)
                   .Select(c => c.Id).SingleOrDefault();

               //1 lijn --> 1 InspraakItem(GemeenteCategorie)  --> 1 Parent -> 1 Parent
                  return ctx.FinLijnen.Include(fin1 => fin1.cat.cat.categorieParent.categorieParent)
                      .Where<FinancieleLijn>(fin1 => fin1.financieelOverzicht.Id == id).Select(

                      fin2 => new DTOfinancieleLijn()
                      {

                          naamCatx = fin2.cat.cat.categorieParent.categorieParent.categorieNaam,
                          naamCaty = fin2.cat.cat.categorieParent.categorieNaam,
                          naamCatz = fin2.cat.cat.categorieNaam,
                          uitgave = fin2.uitgaven,
                          catCode = fin2.cat.cat.categorieCode

                      }
                      );
           }




           public void CreateFinancieleLijnen(IEnumerable<FinancieleLijn> lijnen)
           {
               int count = 0;
               foreach (FinancieleLijn fl in lijnen)
               {
                   ctx.Entry(fl.financieelOverzicht).State = EntityState.Unchanged;
                   CreateNoSaveFinLijn(fl);
                   count++;
               }
               Console.WriteLine(count + " Lines imported");
               ctx.SaveChanges();
               return;
           }


           public Actie ReadActie(string actieCode, string gemeenteNaam)
           {
               return ctx.Acties.Where<Actie>(x => x.actieCode == actieCode).Where<Actie>(x => x.gemeente.naam == gemeenteNaam).SingleOrDefault();
           } */

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

        public void UpdateGemeenteCat(GemeenteCategorie gc)
        {
            ctx.Entry(gc).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        /* public Actie CreateIfNotExistsActie(string actieCode, string actieKort, string actieLang, HoofdGemeente gem, List<Actie> acties)
         {
             List<Actie> actiesSubList = acties.FindAll(x => !HoofdGemeente.Equals(x.gemeente, null));
             Actie actie = actiesSubList.Find(x => x.actieCode.Equals(actieCode) && x.gemeente.naam.Equals(gem.naam));
             if (actie == null)
             {
                 ctx.Entry(gem).State = EntityState.Unchanged;
                 return CreateActie(new Actie(actieCode, actieKort,actieLang, gem));
             }

             return actie;
         } */

        public Actie CreateIfNotExistsActie(string actieKort, string actieLang, List<Actie> acties, BestuurType bt, float inkomsten, float uitgaven, FinancieelOverzicht fo, int gemCatID)
        {

            //nakijken of het om dezelfde actie gaat zoja optellen van inkomsten en uitgaven anders nieuwe actie maken

            GemeenteCategorie gemC = ctx.GemeenteCategorien.Find(gemCatID);
            // inkomsten af trekken om te weten hoeveel de gemeente gaat uitgeven aan deze categorie
            gemC.totaal += gemC.calculateTotal(inkomsten, uitgaven);

            UpdateGemeenteCat(gemC);

            Actie actie = acties.Find(x => x.financieelOverzicht.Id == fo.Id && x.actieKort == actieKort && x.actieLang == actieLang
              && x.bestuurType == bt && x.parentGemCat.ID == gemCatID);
            if (actie == null)
            {
                ctx.Entry(fo).State = EntityState.Unchanged;
                return CreateActie(new Actie(actieKort, actieLang, bt, inkomsten, uitgaven, fo, gemC));
            }

            UpdateActie(actie,inkomsten,uitgaven);
            return actie;

        }

        private void UpdateActie(Actie actie, float inkomsten, float uitgaven)
        {
            actie.inkomsten += inkomsten;
            actie.uitgaven += uitgaven;
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
                // logic to decide if begroting or rekening. naar manager/domain?
                ctx.Entry(gem).State = EntityState.Unchanged;
                bool check = jaar <= DateTime.Now.Year;
                switch (check)
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

       




    }
}
