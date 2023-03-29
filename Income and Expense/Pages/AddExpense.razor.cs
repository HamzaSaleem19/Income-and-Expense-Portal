﻿using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class AddExpense
    {
        [Inject]
        public ExpenseService expenseService { get; set; }
        [Inject]
        public GroupService groupService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        Expense e = new();
        public IEnumerable<string> valuess = new string[] { };
        // public List<Expense> expenseList = new();
        public List<SelectListItem> UserList = new();
        public List<SelectListItem> GroupList = new();
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; set; }
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
