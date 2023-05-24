using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
using Income_and_Expense.View_Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class AddExpense : ComponentBase
    {
        protected bool ShowConfirmation { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; } = "Confirm Delete";

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Are you sure you want to delete";

        public void Show()
        {
            ShowConfirmation = true;
            StateHasChanged();
        }

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        protected async Task OnConfirmationChange(bool value)
        {
            ShowConfirmation = false;
            await ConfirmationChanged.InvokeAsync(value);
        }

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
        private async Task ShowDeleteAlert()
        {
            string message = "Are you sure you want to delete this item?";
            await JSRuntime.InvokeVoidAsync("alert", message);
        }
        protected async void DeleteExpense(int Expense_Id)
        {
            try
            {
                bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
                if (confirmed)
                {
                    await expenseService.DeleteExpenseAsync(Expense_Id);
                    NavigationManager.NavigateTo("AllExpenses", true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
