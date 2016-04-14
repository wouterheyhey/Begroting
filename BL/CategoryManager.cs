using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BL.Domain;


namespace BL
{
    public class CategoryManager
    {
        private readonly CategoryRepository repo;


        public CategoryManager()
        {
            repo = new CategoryRepository();
        }

        public void LoadFinancieleLijnen(int year)
        {
            repo.ImportFinancieleLijnen(year);
        }

        public IEnumerable<Categorie> GetCategorien()
        {
            return repo.ReadCategories();
        }

        public IEnumerable<Gemeente> GetCGemeenten()
        {
            return repo.ReadGemeentes();
        }

        public void SetChildrenCategorien()
        {
            repo.UpdateAllCategoriesChildren();
        }



    }
}
