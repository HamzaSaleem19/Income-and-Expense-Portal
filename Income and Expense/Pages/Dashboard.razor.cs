﻿using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
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
        [Inject]
        public GroupService groupService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        Expense e = new();
        public IEnumerable<string> valuess = new string[] { };
        public List<Expense> expenseList = new();
        public List<SelectListItem> UserList = new();
        public List<SelectListItem> GroupList = new();
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; set; }

        [CascadingParameter]
        public ClaimsPrincipal User { get; set; }

        public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected override async Task OnInitializedAsync()
        {
            UserList = await expenseService.GetAllUsersAsync();
            GroupList = await groupService.GetAllGroupsList();
        }
        public async Task AddData()
        {
            e.UserIds = valuess.ToArray();
            await expenseService.AddExpense(e);
            NavigationManager.NavigateTo("AllExpenses", true);
        }
        void Cancel()
        {
            e = new();
            StateHasChanged();
        }
    }
}
