using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages.NewsArticles
{

    public class LecturerViewModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private const int PageSize = 6;

        public LecturerViewModel(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public IList<NewsArticle> ActiveArticles { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchKeyword { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        private void LoadData()
        {
            var allArticles = _newsArticleRepository.GetActiveArticles();

            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                allArticles = allArticles.Where(x => x.NewsTitle.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            TotalPages = (int)Math.Ceiling(allArticles.Count / (double)PageSize);

            ActiveArticles = allArticles
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }

}
