using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class FileLocator
    {
        private const string libRelDir = @"C:\lib\";

        internal string findExcelSourceDir()
        {
            // Gebruik van Path functies om relatieve paden vanuit de Debug folder te vermijden
            string path = Path.GetDirectoryName(Path.GetDirectoryName(
                                                       Path.GetDirectoryName(
                                                       Assembly.GetExecutingAssembly().GetName().CodeBase)));

            path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string relativePath = libRelDir;    // Relatief t.o.v. the CA
            string importPath = Path.GetFullPath(Path.Combine(path, relativePath));
            return importPath;
        }
    }
}
