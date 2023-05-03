using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class AddGroups
    {
        Groups g = new();
        public List<Groups> groupobj = new();
        public List<UserGroup> userGroups = new();
        public List<SelectListItem> grouplist = new();
        public IEnumerable<string> valuess = new string[] { };
        Expense e = new();
        public List<SelectListItem> UserList = new();
        public List<SelectListItem> UserListG = new();
        public List<SelectListItem> GroupList = new();
        [Inject]
        public ExpenseService expenseService { get; set; }



        protected override async Task OnInitializedAsync()
        {
            userGroups = await groupService.GetAllUserGroups();
            groupobj = await groupService.GetAllGroups();
            grouplist = await groupService.GetAllUsers();
            UserList = await expenseService.GetAllUsersAsync();
            GroupList = await groupService.GetAllGroupsList();
        }


        //public Groups group = new();
        //protected async void InsertGroup()
        //{

        //    await groupService.InsertGroup(group);
        //    NavigationManager.NavigateTo("/AddGroup", true);

        //}

        void cancel()
        {
            g = new();
            StateHasChanged();
        }
        protected async void UpdateGroup(int groupid)
        {
            g = await groupService.GetGroups(groupid);
               StateHasChanged();
        }

        protected async void DeleteGroup(int Groupid)
        {
            try
            {
                await groupService.DeleteGroupsAsync(Groupid);
                NavigationManager.NavigateTo("AddGroup", true);
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddData()
        {
            g.UserIds = valuess.ToArray();
            await groupService.InsertGroup(g);
            NavigationManager.NavigateTo("/AddGroup", true);
        }

        public async Task AddExpenseData()
        {
            e.UserIds = valuess.ToArray();
            await expenseService.AddExpense(e);
            NavigationManager.NavigateTo("/", true);
        }

        public async void Getusersgroupwise(int? groupid)
        {
            UserListG = new();
            e.Group_Id = groupid;
            UserListG = await groupService.GetUserbygroupid(groupid);
            StateHasChanged();
        }
    }
}
