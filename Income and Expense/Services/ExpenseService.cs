﻿using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Income_and_Expense.Services
{
    public class ExpenseService: ComponentBase
    {
        private readonly AuthenticationStateProvider UserauthenticationStateProvider;


        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        
        public ExpenseService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext _context, AuthenticationStateProvider 
            _UserauthenticationStateProvider)
        {
            _roleManager = roleManager;
            this.userManager = userManager;
            context= _context;
            this.UserauthenticationStateProvider = _UserauthenticationStateProvider;
        }
       
        public async Task<List<SelectListItem>> GetAllUsersAsync()
        {
            AuthenticationState authState = await UserauthenticationStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = authState.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userName = user.FindFirstValue(ClaimTypes.Name);
            var userslist = await userManager.Users.Select(x =>
                 new SelectListItem()
                 {
                     Value = x.Id,
                     Text = x.FirstName+" "+x.LastName
                 }).ToListAsync();
            return userslist;
        }
        public async Task<bool> AddExpense(Expense expense)
        {
            await context.Expenses.AddAsync(expense);
            List<ManageExpense> ListOfmanageExpense = new();
            foreach (var item in expense.UserIds)
            {
                ManageExpense manageExpense = new ManageExpense();
                manageExpense.User_Id = item;
                manageExpense.Expense = expense;
                manageExpense.Amount = expense.Amount/expense.UserIds.Count();
                ListOfmanageExpense.Add(manageExpense);
                //var lent = expense.Amount - manageExpense.Amount;
            }
           
            await context.ManageExpenses.AddRangeAsync(ListOfmanageExpense);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Expense>> GetAllExpenses()
        {
            try
            {
                return await context.Expenses.ToListAsync();
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public async Task<bool> DeleteExpenseAsync(int Expense_Id)//delete expense
        {
            var checkexprecord = await context.Expenses.Where(x => x.Expense_Id == Expense_Id).FirstOrDefaultAsync();
            if (checkexprecord != null)
            {
                context.Expenses.Remove(checkexprecord);
                await context.SaveChangesAsync();
            }

            return true;
        }
    }
}
