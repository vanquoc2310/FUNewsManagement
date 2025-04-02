using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public IndexModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<Category> AllCategories { get; set; } = new List<Category>();

        public bool IsPopupOpen { get; set; } = false;
        public bool IsDeletePopup { get; set; } = false;
        public bool IsEditMode { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        public short IdToDelete { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        public void OnGetCreatePopup()
        {
            IsPopupOpen = true;
            IsEditMode = false;
            Category = new Category { IsActive = true };
            LoadData();
        }

        public void OnGetEditPopup(short id)
        {
            Category = _categoryRepository.GetCategoryById(id);
            if (Category == null)
            {
                TempData["ToastMessage"] = "❌ Không tìm thấy category!";
                Response.Redirect($"?CurrentPage={CurrentPage}");
                return;
            }

            IsPopupOpen = true;
            IsEditMode = true;
            LoadData();
        }

        public void OnGetDeleteConfirm(short id)
        {
            IdToDelete = id;
            IsDeletePopup = true;
            LoadData();
        }

        public IActionResult OnPostSave()
        {
            if (!ModelState.IsValid)
            {
                IsPopupOpen = true;
                IsEditMode = Category.CategoryId != 0;
                LoadData();
                return Page();
            }

            bool isNew = Category.CategoryId == 0;
            bool result = isNew
                ? _categoryRepository.CreateCategory(Category)
                : _categoryRepository.UpdateCategory(Category);

            TempData["ToastMessage"] = result
                ? (isNew ? "✅ Tạo category thành công!" : "✅ Cập nhật category thành công!")
                : "❌ Có lỗi xảy ra khi lưu category!";

            return RedirectToPage(new { CurrentPage });
        }

        public IActionResult OnPostDelete(short id)
        {
            var success = _categoryRepository.DeleteCategory(id);
            TempData["ToastMessage"] = success
                ? "✅ Xóa category thành công!"
                : "❌ Không thể xóa category này! Danh mục đang chứa bài viết.";

            return RedirectToPage(new { CurrentPage });
        }

        private void LoadData()
        {
            var all = _categoryRepository.GetAllCategories();
            AllCategories = all;

            int totalItems = all.Count;
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (CurrentPage < 1) CurrentPage = 1;
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;

            Categories = all
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
