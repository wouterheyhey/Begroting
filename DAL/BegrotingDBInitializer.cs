using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Annotations;
using System.Reflection;

namespace DAL
{
    internal class BegrotingDBInitializer : DropCreateDatabaseAlways<BegrotingDBContext>
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


        }

    }
}
