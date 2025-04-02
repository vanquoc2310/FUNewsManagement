using FUNewsManagement_BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_DAO
{
    public class CategoryDAO
    {
        private FunewsManagementContext _context;
        private static CategoryDAO instance;

        public CategoryDAO()
        {
            if (_context == null) _context = new();
        }

        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new();
                }
                return instance;
            }
        }

        public IList<Category> GetAllCategories()
        {
            return _context.Categories.Include(c => c.ParentCategory).ToList();
        }

        public bool CreateCategory(Category newCategory)
        {
            newCategory.IsActive = true;
            _context.Categories.Add(newCategory);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateCategory(Category updatedCategory)
        {
            // Check nếu đã có entity trùng ID đang bị tracked
            var local = _context.Categories.Local
                .FirstOrDefault(entry => entry.CategoryId == updatedCategory.CategoryId);

            if (local != null)
            {
                // Hủy theo dõi entity cũ trước khi cập nhật
                _context.Entry(local).State = EntityState.Detached;
            }

            // Gán lại trạng thái modified cho entity truyền vào
            _context.Attach(updatedCategory);
            _context.Entry(updatedCategory).State = EntityState.Modified;

            var result = _context.SaveChanges();
            Console.WriteLine($"💾 Rows affected (Category): {result}");

            return result > 0;
        }

        public Category GetCategoryByCategoryName(string name)
        {
            return _context.Categories.Include(c => c.ParentCategory)
                .FirstOrDefault(x => x.CategoryName.ToLower().Equals(name.ToLower()));
        }

        public Category GetCategoryById(short id)
        {
            return _context.Categories.Include(c => c.ParentCategory)
                .FirstOrDefault(x => x.CategoryId == id);
        }

        public bool DeleteCategory(short id)
        {
            if (id == 0) return false;

            // Kiểm tra ràng buộc: danh mục có bài viết hay không
            var checkNews = _context.NewsArticles.Any(x => x.CategoryId == id);
            if (checkNews) return false;

            // Hủy theo dõi local entity nếu đang bị tracked
            var local = _context.Categories.Local
                .FirstOrDefault(entry => entry.CategoryId == id);

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            // Tạo entity tạm với ID rồi attach để xóa
            var category = new Category { CategoryId = id };
            _context.Attach(category);
            _context.Categories.Remove(category);

            return _context.SaveChanges() > 0;
        }
    }
}
