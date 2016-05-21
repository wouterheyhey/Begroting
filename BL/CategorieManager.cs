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
        //    repo.UpdateAllCategoriesChildren();
        }

        public IEnumerable<InspraakItem> GetAllChildren(InspraakItem ii, List<InspraakItem> children)
        {
            List<InspraakItem> chldren = repo.GetChildrenInspraakItem(ii).ToList<InspraakItem>();
            foreach (InspraakItem child in chldren)
            {
                children.Add(child);
                if (chldren.Count() > 0)
                {
                    return GetAllChildren(child, children);  // recursie
                }
                // else { return children; }
            }
            return children;
        }

        public IEnumerable<InspraakItem> GetChildrenInspraakItem(InspraakItem ii)
        {
            List<InspraakItem> children = repo.GetChildrenInspraakItem(ii).ToList<InspraakItem>();
            return children;
        }

        public IEnumerable<InspraakItem> GetAllParents(InspraakItem ii, List<InspraakItem> parents)
        {
            List<InspraakItem> prnts = repo.GetParentsInspraakItem(ii).ToList<InspraakItem>();
            foreach (InspraakItem child in prnts)
            {
                parents.Add(child);
                if (prnts.Count() > 0)
                {
                    return GetAllParents(child, parents);  // recursie
                }
                // else { return children; }
            }
            return parents;
        }

        public IEnumerable<GemeenteCategorie> GetHighestLevelCats()
        {
            return repo.GetHighestLevelCats().ToList<GemeenteCategorie>();
        }




    }




}

