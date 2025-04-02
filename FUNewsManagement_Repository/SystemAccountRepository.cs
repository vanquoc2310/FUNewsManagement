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
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public SystemAccount GetAccount(string email, string password) => SystemAccountDAO.Instance.GetAccount(email, password);

        public IList<SystemAccount> GetAllAccounts() => SystemAccountDAO.Instance.GetAllAccounts();
        public SystemAccount GetAccountById(short id) => SystemAccountDAO.Instance.GetAccountById(id);

        public void DeleteAccount(short id) => SystemAccountDAO.Instance.DeleteAccount(id);

        public bool UpdateAccount(SystemAccount account) => SystemAccountDAO.Instance.UpdateAccount(account);
    }
}
