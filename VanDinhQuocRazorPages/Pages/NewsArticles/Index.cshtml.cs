using FUNewsManagement_BusinessObjects;
using FUNewsManagement_DAO;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VanDinhQuocRazorPages.Hubs;

namespace VanDinhQuocRazorPages.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IHubContext<NewsHub> _hubContext;
        private readonly FunewsManagementContext _context;

        public IndexModel(
            INewsArticleRepository newsArticleRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            IHubContext<NewsHub> hubContext,
            FunewsManagementContext context)
        {
            _newsArticleRepository = newsArticleRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _hubContext = hubContext;
            _context = context;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = new();

        public IList<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<Tag> AllTags { get; set; } = new List<Tag>();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        public bool IsPopupOpen { get; set; } = false;
        public bool IsEditMode { get; set; } = false;
        public bool IsDeletePopup { get; set; } = false;
        public string IdToDelete { get; set; } = string.Empty;

        public void OnGet()
        {

            IsPopupOpen = false;
            IsEditMode = false;
            IsDeletePopup = false;

            LoadData();
        }

        public void OnGetCreatePopup()
        {
            NewsArticle = new();
            IsPopupOpen = true;
            IsEditMode = false;

            LoadData();
        }

        public void OnGetEditPopup(string id)
        {
            NewsArticle = _newsArticleRepository.GetArticleById(id);
            if (NewsArticle != null)
            {
                SelectedTagIds = NewsArticle.Tags.Select(t => t.TagId).ToList();
                IsPopupOpen = true;
                IsEditMode = true;
            }
            LoadData();
        }

        public void OnGetDeleteConfirm(string id)
        {
            IdToDelete = id;
            IsDeletePopup = true;
            LoadData();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var result = _newsArticleRepository.DeleteNewsArticle(id);
                TempData["ToastMessage"] = result ? "✅ Xoá thành công!" : "❌ Xoá thất bại!";
                TempData["ActionSuccess"] = result; // ✅ Thêm dòng này để cho phép UI xử lý toast

                if (result)
                    await _hubContext.Clients.All.SendAsync("ReceiveUpdate");
            }

            return RedirectToPage(new { CurrentPage });
        }


        public async Task<IActionResult> OnPostSaveAsync()
        {
            var userIdStr = HttpContext.Session.GetString("Id");
            if (!short.TryParse(userIdStr, out short userId))
            {
                TempData["ToastMessage"] = "Không thể lấy thông tin người dùng.";
                IsPopupOpen = true;
                return Page();
            }

            bool isCreate = string.IsNullOrEmpty(NewsArticle.NewsArticleId);

            if (isCreate)
            {
                var maxId = _newsArticleRepository.GetAllArticles()
                    .Select(x => int.TryParse(x.NewsArticleId, out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();

                NewsArticle.NewsArticleId = (maxId + 1).ToString();
                NewsArticle.CreatedDate = DateTime.Now;
                NewsArticle.NewsStatus = true;
                NewsArticle.CreatedById = userId;
            }

            NewsArticle.ModifiedDate = DateTime.Now;
            NewsArticle.UpdatedById = userId;


            NewsArticle.Tags = new List<Tag>();
            foreach (var tagId in SelectedTagIds)
            {
                var localTag = _context.Tags.Local.FirstOrDefault(t => t.TagId == tagId);
                Tag tagToUse = localTag ?? _context.Tags.Find(tagId);
                if (tagToUse != null)
                    NewsArticle.Tags.Add(tagToUse);
            }

            bool result = isCreate
                ? _newsArticleRepository.CreateNewsArticle(NewsArticle)
                : _newsArticleRepository.UpdateNewsArticle(NewsArticle);

            TempData["ToastMessage"] = result
                ? (isCreate ? "✅ Tạo mới thành công!" : "✅ Cập nhật thành công!")
                : "❌ Lưu thất bại.";
            TempData["ActionSuccess"] = result;

            if (result)
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate");

            return RedirectToPage(new { CurrentPage });
        }

        private void LoadData()
        {
            Categories = _categoryRepository.GetAllCategories();
            AllTags = _tagRepository.GetAll();

            var articles = _newsArticleRepository.GetAllArticles();
            int totalItems = articles.Count;
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages);
            NewsArticles = articles.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}