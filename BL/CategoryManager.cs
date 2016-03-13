using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL
{
    public class CategoryManager
    {
        private readonly CategoryRepository repo;
        public CategoryManager()
        {
            repo = new CategoryRepository();
        }

    }
}
