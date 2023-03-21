using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class AddExpense
    {
        Expense e = new();
        public List<Expense> expenseList = new();
        public List<SelectListItem> UserList = new();
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; set; }
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
