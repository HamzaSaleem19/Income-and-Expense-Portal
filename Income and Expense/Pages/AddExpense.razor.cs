using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
using Income_and_Expense.View_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
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
        [Inject]
        private  UserManager<ApplicationUser> _userManager { get; set; }


        Expense e = new();
        public IEnumerable<string> valuess = new string[] { };
        public List<ManageVM> expenseList = new();
        public List<ManageExpense> manageExpenseList = new();
        public List<SelectListItem> UserList = new();
        public List<SelectListItem> GroupList = new();
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            manageExpenseList = await expenseService.GetAllManageExpenses();
            expenseList = await expenseService.GetAllExpenses();
            //var list = await expenseService.GetAllExpenses();
            UserList = await expenseService.GetAllUsersAsync();
            GroupList = await groupService.GetAllGroupsList();
        }
        public async Task AddData()
        {
            var tt =await GetUserName("");


            e.UserIds = valuess.ToArray();
            await expenseService.AddExpense(e);
            NavigationManager.NavigateTo("AllExpenses", true);
        }
        void Cancel()
        {
            e = new();
            StateHasChanged();
        }

        public async Task<string> GetUserName(string userId)
        {
            var user= await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            var username =  (user == null ? "" : user.FirstName + " " + user.LastName);
            return username;
        }

        protected async void DeleteExpense(int Expense_Id)
        {
            try
            {
                await expenseService.DeleteExpenseAsync(Expense_Id);
                NavigationManager.NavigateTo("AllExpenses", true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
