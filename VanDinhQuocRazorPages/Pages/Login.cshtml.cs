using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages
{
    public class LoginModel : PageModel
    {
        public string ErrorMessage { get; set; } = string.Empty;

        private static ISystemAccountRepository _systemAccountRepository;
        public LoginModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            string txtEmail = Request.Form["txtEmail"];
            string txtPassword = Request.Form["txtPassword"];

            SystemAccount account = _systemAccountRepository.GetAccount(txtEmail, txtPassword);

            if (account == null)
            {
                ErrorMessage = "You do not have permission to do this function!";
                return;
            }

            HttpContext.Session.SetString("Role", account.AccountRole.ToString());
            HttpContext.Session.SetString("Name", account.AccountName);
            HttpContext.Session.SetString("Id", account.AccountId.ToString());

            if (account.AccountRole == 1)
            {
                Response.Redirect("/Categories");
            }
            else if (account.AccountRole == 2)
            {
                Response.Redirect("/NewsArticles/LecturerViewModel");
            }
            else if (account.AccountRole == 3)
            {
                Response.Redirect("/SystemAccounts");
            }

        }
    }
}
