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
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly NewsArticleDAO _dao;
        public NewsArticleRepository(FunewsManagementContext context)
        {
            _dao = new NewsArticleDAO(context);
        }

        public bool CreateNewsArticle(NewsArticle newsArticle) => _dao.CreateNewsArticle(newsArticle);

        public bool DeleteNewsArticle(string id) => _dao.DeleteNewsArticle(id);
        public IList<NewsArticle> GetActiveArticles() => _dao.GetActiveArticles();

        public IList<NewsArticle> GetAllArticles() => _dao.GetAllArticles();

        public NewsArticle GetArticleById(string id) => _dao.GetById(id);
        public IList<NewsArticle> GetHistoryArticles(short id) => _dao.GetHistoryArticles(id);

        public bool UpdateNewsArticle(NewsArticle updatedArticle) => _dao.UpdateNewsArticle(updatedArticle);
        public IList<NewsArticle> GetArticlesByDateRange(DateTime startDate, DateTime endDate)
    => _dao.GetArticlesByDateRange(startDate, endDate);
    }
}
