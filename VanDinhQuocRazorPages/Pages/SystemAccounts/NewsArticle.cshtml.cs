using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages.SystemAccounts
{
    public class NewsArticleModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepo;

        public NewsArticleModel(INewsArticleRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }

        private const int PageSize = 5;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }


        public int TotalPages { get; set; }
        public IList<NewsArticle> FilteredArticles { get; set; } = new List<NewsArticle>();
        public IList<NewsArticle> PaginatedArticles { get; set; } = new List<NewsArticle>();

        public void OnGet()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                FilteredArticles = _newsRepo.GetArticlesByDateRange(StartDate.Value, EndDate.Value);
            }
            else
            {
                FilteredArticles = _newsRepo.GetAllArticles()
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();
            }

            TotalPages = (int)Math.Ceiling(FilteredArticles.Count / (double)PageSize);

            PaginatedArticles = FilteredArticles
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
