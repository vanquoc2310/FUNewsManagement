using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages.NewsHistory
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public IndexModel(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public IList<NewsArticle> NewsHistory { get; set; } = new List<NewsArticle>();

        public IActionResult OnGet()
        {
            var userIdStr = HttpContext.Session.GetString("Id");
            if (!short.TryParse(userIdStr, out short userId)) return RedirectToPage("/Login");

            NewsHistory = _newsArticleRepository.GetHistoryArticles(userId);
            return Page();
        }
    }
}
