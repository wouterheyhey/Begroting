using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BL.Domain;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;
using System.Reflection;
using DAL.repositories;
using Microsoft.AspNet.Identity;


namespace DAL
{
    internal class BegrotingDBInitializer : DropCreateDatabaseIfModelChanges<BegrotingDBContext>
    {


        protected override void Seed(BegrotingDBContext ctx)
        {
            string importPath = (new FileLocator()).findExcelSourceDir();
            string categoryFileCat = "Config_Categorien.xls";
            string categoryFileGem = "Config_Gemeenten.xls";
            string categoryFileClu = "Config_Clusters.xls";
            string categoryFilePos = "Config_postcodesHoofdgemeentes.xlsx";

            base.Seed(ctx);

            foreach (HoofdGemeente s in ExcelImporter.ImportHoofdGemeenten(importPath + categoryFileGem, importPath + categoryFileClu, importPath + categoryFilePos))
            {
                System.Diagnostics.Debug.WriteLine(s.ToString());
                ctx.Gemeenten.Add(s);
            }
            ctx.SaveChanges();

            foreach (Categorie s in ExcelImporter.ImportCategories(importPath + categoryFileCat).Values)
            {
                System.Diagnostics.Debug.WriteLine(s.ToString());
                ctx.Categorien.Add(s);
            }
            ctx.SaveChanges();

            CreateSystemUser();
        }

        private void CreateSystemUser()
        {
            AccountRepository accRepo = new AccountRepository();
            GemeenteRepository gemRepo = new GemeenteRepository();
            InTeLoggenGebruiker gebruiker = new InTeLoggenGebruiker()
            {
                email = CredentialHandler.Decrypt("synT9FqJUvjvKVuXpo0rlrY6sbhEnR7sdb8psU4wACjsuPrenWJhH0IriM3U8cFPs8cyTetZeQTZSMqFQoLp"),
                Password = CredentialHandler.Decrypt("vvfK5GPP6Z8Fihbet4XyJ7lFSfE+We61O82U/iW9lY8+rtYM82xJJIWVRtxdDJc="),
                bevestigPaswoord = CredentialHandler.Decrypt("vvfK5GPP6Z8Fihbet4XyJ7lFSfE+We61O82U/iW9lY8+rtYM82xJJIWVRtxdDJc="),
                Naam = CredentialHandler.Decrypt("lFLEFqjYLG2xw2T5IUV1AyJXN3euBipwLvI8+zKUo0aUD+KyQRzo21liuXDa9T8vPAKHXgdaVq3pZje/Uafa"),
                gemeente = CredentialHandler.Decrypt("gh3YSAHrOP3PxTub9zjL1hGl6n8FdKRiPMfKHY0y+UGho547KBdVSKhBq0Yr7vE=")
            };
            var result = accRepo.RegisterUser(gebruiker, gemRepo.ReadGemeente(gebruiker.gemeente), RolType.superadmin);
        }
    }
}
