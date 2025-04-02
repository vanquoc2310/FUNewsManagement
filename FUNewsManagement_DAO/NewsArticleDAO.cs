using FUNewsManagement_BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_DAO
{
    public class NewsArticleDAO
    {
        private readonly FunewsManagementContext _context;
        public NewsArticleDAO(FunewsManagementContext context)
        {
            _context = context;
        }

        public IList<NewsArticle> GetActiveArticles()
        {
            return _context.NewsArticles
                .Where(x => x.NewsStatus == true)
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags)
                .ToList();
        }

        public IList<NewsArticle> GetAllArticles()
        {
            return _context.NewsArticles
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags)
                .Include(x => x.Category)
                .ToList();
        }

        public IList<NewsArticle> GetHistoryArticles(short id)
        {
            return _context.NewsArticles
                .Where(x => x.CreatedById == id)
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags)
                .ToList();
        }

        public NewsArticle GetById(string id)
        {
            return _context.NewsArticles
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.NewsArticleId.Equals(id));
        }

        public bool CreateNewsArticle(NewsArticle newsArticle)
        {
            _context.NewsArticles.Add(newsArticle); // ✅ Dùng trực tiếp object đã có Tags được tracking
            return _context.SaveChanges() > 0;
        }




        public bool UpdateNewsArticle(NewsArticle updatedArticle)
        {
            var existingArticle = _context.NewsArticles
                .Include(a => a.Tags)
                .FirstOrDefault(a => a.NewsArticleId == updatedArticle.NewsArticleId);

            if (existingArticle == null)
                return false;

            // Cập nhật các field cơ bản
            existingArticle.NewsTitle = updatedArticle.NewsTitle;
            existingArticle.NewsContent = updatedArticle.NewsContent;
            existingArticle.Headline = updatedArticle.Headline;
            existingArticle.NewsSource = updatedArticle.NewsSource;
            existingArticle.ModifiedDate = updatedArticle.ModifiedDate;
            existingArticle.NewsStatus = updatedArticle.NewsStatus;
            existingArticle.CategoryId = updatedArticle.CategoryId;
            existingArticle.UpdatedById = updatedArticle.UpdatedById;

            // Cập nhật Tags
            existingArticle.Tags.Clear(); // Xoá toàn bộ tag cũ
            foreach (var tag in updatedArticle.Tags)
            {
                if (_context.Entry(tag).State == EntityState.Detached)
                    _context.Attach(tag);

                _context.Entry(tag).State = EntityState.Unchanged;
                existingArticle.Tags.Add(tag);
            }

            return _context.SaveChanges() > 0;
        }

        public bool DeleteNewsArticle(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;

            var article = _context.NewsArticles
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.NewsArticleId.Equals(id));

            if (article == null) return false;

            article.Tags.Clear(); // xoá liên kết nhiều-nhiều với Tag
            _context.NewsArticles.Remove(article);

            return _context.SaveChanges() > 0;
        }

        public IList<NewsArticle> GetArticlesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.NewsArticles
                .Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate)
                .Include(x => x.Category)
                .Include(x => x.CreatedBy)
                .Include(x => x.Tags)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
        }

    }
}
