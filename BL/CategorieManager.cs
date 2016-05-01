using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL.repositories;

namespace BL
{
    public class CategorieManager
    {
        private readonly CategorieRepository repo;


        public CategorieManager()
        {
            repo = new CategorieRepository();
        }    

        public IEnumerable<Categorie> GetCategorien()
        {
            return repo.ReadCategories();
        }

        public void SetChildrenCategorien()
        {
            repo.UpdateAllCategoriesChildren();
        }





    }
}
