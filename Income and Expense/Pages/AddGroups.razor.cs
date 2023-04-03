using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class AddGroups
    {
        Groups g = new();
        public List<Groups> groupobj = new();
        public List<SelectListItem> grouplist = new();
        public IEnumerable<string> valuess = new string[] { };



        protected override async Task OnInitializedAsync()
        {
            groupobj = await groupService.GetAllGroups();
            grouplist = await groupService.GetAllUsers();
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

    }
}
