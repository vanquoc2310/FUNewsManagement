using FUNewsManagement_BusinessObjects;
using FUNewsManagement_Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VanDinhQuocRazorPages.Pages.SystemAccounts
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        private const int PageSize = 5;

        public IndexModel(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;
        }

        public IList<SystemAccount> SystemAccount { get; set; } = default!;
        public IList<SystemAccount> PaginatedAccounts { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int? RoleFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchKeyword { get; set; }

        [BindProperty]
        public SystemAccount EditableAccount { get; set; } = default!;

        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        private void LoadData()
        {
            // Lấy toàn bộ dữ liệu
            SystemAccount = _systemAccountRepository.GetAllAccounts();

            // Filter theo role nếu có
            if (RoleFilter.HasValue)
            {
                SystemAccount = SystemAccount
                    .Where(x => x.AccountRole == RoleFilter.Value)
                    .ToList();
            }

            // Search theo tên nếu có từ khóa
            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                SystemAccount = SystemAccount
                    .Where(x => x.AccountName != null && x.AccountName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Tính tổng số trang
            TotalPages = (int)Math.Ceiling(SystemAccount.Count / (double)PageSize);

            // Lấy danh sách theo trang hiện tại
            PaginatedAccounts = SystemAccount
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public IActionResult OnGetDeleteAccount(short id)
        {
            var account = _systemAccountRepository.GetAccountById(id);
            if (account == null)
            {
                TempData["Error"] = "Tài khoản không tồn tại.";
                return RedirectToPage();
            }

            _systemAccountRepository.DeleteAccount(id);
            TempData["Success"] = "Xóa tài khoản thành công.";
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateAccount()
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Thông tin không hợp lệ.";
                LoadData();
                return Page();
            }

            bool updated = _systemAccountRepository.UpdateAccount(EditableAccount);

            if (updated)
            {
                TempData["Success"] = "Cập nhật thành công.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy tài khoản hoặc cập nhật thất bại.";
            }

            return RedirectToPage(new { CurrentPage, RoleFilter, SearchKeyword });
        }
    }
}
