using FUNewsManagement_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagement_Repository.Interfaces
{
    public interface ISystemAccountRepository
    {
        SystemAccount GetAccount(string email, string password);
        IList<SystemAccount> GetAllAccounts();
        SystemAccount GetAccountById(short id);
        void DeleteAccount(short id);
        bool UpdateAccount(SystemAccount account);
    }
}
