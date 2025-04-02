using FUNewsManagement_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_Repository.Interfaces
{
    public interface INewsArticleRepository
    {
        IList<NewsArticle> GetActiveArticles();
        IList<NewsArticle> GetAllArticles();
        IList<NewsArticle> GetHistoryArticles(short id);
        NewsArticle GetArticleById(string id);
        bool CreateNewsArticle(NewsArticle newsArticle);
        bool UpdateNewsArticle(NewsArticle updatedArticle);
        bool DeleteNewsArticle(string id);
        IList<NewsArticle> GetArticlesByDateRange(DateTime startDate, DateTime endDate);

    }
}
