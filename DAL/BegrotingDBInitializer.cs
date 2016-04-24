﻿using System;
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
            string categoryFileCat = "Config_Categorien.xls";
            string categoryFileGem = "Config_Gemeenten.xls";
            string categoryFileClu = "Config_Clusters.xls";
            string categoryFilePos = "Config_postcodesHoofdgemeentes.xlsx";


            base.Seed(ctx);

            // foreach (Cluster s in ExcelImporter.ImportClusters(importPath + categoryFileClu))
            // {
            //     Console.WriteLine(s.ToString());
            ////     ctx.Clusters.Add(s);
            // }
            //// ctx.SaveChanges();
            // Console.ReadLine();

            foreach (HoofdGemeente s in ExcelImporter.ImportHoofdGemeenten(importPath + categoryFileGem, importPath + categoryFileClu, importPath + categoryFilePos))
            {
                Console.WriteLine(s.ToString());
                ctx.Gemeenten.Add(s);
            }
            ctx.SaveChanges();
            Console.ReadLine();


            foreach (Categorie s in ExcelImporter.ImportCategories(importPath + categoryFileCat).Values)
            {
                Console.WriteLine(s.ToString());
                ctx.Categorien.Add(s);
            }
            ctx.SaveChanges();
            Console.ReadLine();


        }

    }
}
