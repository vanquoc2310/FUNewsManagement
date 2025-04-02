using FUNewsManagement_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_Repository.Interfaces
{
    public interface ICategoryRepository
    {
        IList<Category> GetAllCategories();
        Category GetCategoryByCategoryName(string name);
        Category GetCategoryById(short id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(short id);
    }
}
