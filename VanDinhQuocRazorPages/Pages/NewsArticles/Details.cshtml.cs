using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages.NewsArticles
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public DetailsModel(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public NewsArticle NewsArticle { get; set; } = default!;

        public void OnGet(string newsArticleId)
        {
            if (string.IsNullOrEmpty(newsArticleId))
            {
                Response.Redirect("/Error");
            }

            
            NewsArticle = _newsArticleRepository.GetArticleById(newsArticleId);

            if (NewsArticle == null)
            {
                Response.Redirect("/Error");
            }
        }
    }
}
