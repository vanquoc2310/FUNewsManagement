using FUNewsManagement_BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_DAO
{
    public class SystemAccountDAO
    {
        private FunewsManagementContext _context;
        private static SystemAccountDAO instance;

        public SystemAccountDAO()
        {
            if (_context == null)
                _context = new();
        }

        public static SystemAccountDAO Instance
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

        public SystemAccount GetAccount(string email, string password)
        {
            return _context.SystemAccounts
                .Where(x => x.AccountEmail.Equals(email) && x.AccountPassword.Equals(password))
                .FirstOrDefault();
        }

        public IList<SystemAccount> GetAllAccounts()
        {
            return _context.SystemAccounts.ToList();
        }

        public SystemAccount GetAccountById(short id)
        {
            return _context.SystemAccounts
                .Where(x => x.AccountId == id)
                .FirstOrDefault();
        }

        public void DeleteAccount(short id)
        {
            var account = _context.SystemAccounts.Include(x => x.NewsArticles).FirstOrDefault(x => x.AccountId == id);

            //var newArticles = _context.NewsArticles.Where(x => x.CreatedById == id);
            //         foreach (var item in newArticles)
            //         {
            //	item.NewsStatus = false;
            //         }
            account.NewsArticles.Clear();

            _context.SystemAccounts.Remove(account);
            _context.SaveChanges();
        }

        public bool UpdateAccount(SystemAccount account)
        {
            // Check nếu đã có entity trùng ID đang bị tracked
            var local = _context.SystemAccounts.Local
                .FirstOrDefault(entry => entry.AccountId == account.AccountId);

            if (local != null)
            {
                // Hủy theo dõi entity cũ trước khi cập nhật
                _context.Entry(local).State = EntityState.Detached;
            }

            // Gán lại trạng thái modified cho entity truyền vào
            _context.Attach(account);
            _context.Entry(account).State = EntityState.Modified;

            // Đảm bảo có mật khẩu
            account.AccountPassword ??= "@1";

            var result = _context.SaveChanges();
            Console.WriteLine($"💾 Rows affected: {result}");

            return result > 0;
        }

    }
}
