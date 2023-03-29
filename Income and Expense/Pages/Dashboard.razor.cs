using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class Dashboard
    {
        Expense e = new();
        public List<Expense> expenseList = new();
        public List<SelectListItem> UserList = new();
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; set; }

        [CascadingParameter]
        public ClaimsPrincipal User { get; set; }

        public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected override async Task OnInitializedAsync()
        {
            UserList = await expenseService.GetAllUsersAsync();
        }
        public async Task AddData()
        {
            await expenseService.AddExpense(e);
        }
    }
}
