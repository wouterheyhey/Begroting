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

namespace DAL
{
    internal class BegrotingDBInitializer : DropCreateDatabaseAlways<BegrotingDBContext>
    {


        protected override void Seed(BegrotingDBContext ctx)
        {
            string importPath = @"..\..\..\DAL\lib\";
            string categoryFile = "Config_Categorien.xls";

            base.Seed(ctx);
            foreach (Categorie s in ExcelImporter.ImportCategories(importPath + categoryFile).Values)
            {
                Console.WriteLine(s.ToString());
                ctx.Categorien.Add(s);
            }
            ctx.SaveChanges();
            Console.ReadLine();


        }

    }
}
