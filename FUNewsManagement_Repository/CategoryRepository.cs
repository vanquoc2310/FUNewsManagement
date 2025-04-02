using FUNewsManagement_BusinessObjects;
using FUNewsManagement_DAO;
using FUNewsManagement_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public bool CreateCategory(Category category) => CategoryDAO.Instance.CreateCategory(category);

        public bool DeleteCategory(short id) => CategoryDAO.Instance.DeleteCategory(id);

        public IList<Category> GetAllCategories() => CategoryDAO.Instance.GetAllCategories();

        public Category GetCategoryByCategoryName(string name) => CategoryDAO.Instance.GetCategoryByCategoryName(name);

        public Category GetCategoryById(short id) => CategoryDAO.Instance.GetCategoryById(id);

        public bool UpdateCategory(Category category) => CategoryDAO.Instance.UpdateCategory(category);
    }
}
