using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.View_Models;
using Income_and_Expense.ViewModels;
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
            expense.Lent = Math.Round( expense.Amount - (expense.Amount / expense.UserIds.Count()));

            await context.Expenses.AddAsync(expense);
            List<ManageExpense> ListOfmanageExpense = new();
            foreach (var item in expense.UserIds)
            {
                ManageExpense manageExpense = new ManageExpense();
                manageExpense.User_Id = item;
                manageExpense.Expense = expense;
                manageExpense.Amount = expense.Amount/expense.UserIds.Count();
                //expense.Lent = expense.Amount - manageExpense.Amount;
                ListOfmanageExpense.Add(manageExpense);
                //var lent = expense.Amount - manageExpense.Amount;
            }
           
            await context.ManageExpenses.AddRangeAsync(ListOfmanageExpense);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ManageVM>> GetAllExpenses()
        {
            try
            {
                AuthenticationState authState = await UserauthenticationStateProvider.GetAuthenticationStateAsync();
                ClaimsPrincipal user = authState.User;
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var listofExpenses= await context.ManageExpenses.ToListAsync();
                var managelist = (from e in context.Expenses
                                  join em in context.ManageExpenses on e.Expense_Id equals em.Expense_Id
                                  where e.Paidby == userId && em.User_Id != userId

                                  select new ManageVM
                                  {
                                      TotalBalance = context.ManageExpenses.Where(x => x.Expense_Id == e.Expense_Id && x.User_Id != userId).Select(x => x.Amount).Sum(),
                                      Paidby = e.Paidby,
                                      expenseId = e.Expense_Id,
                                      splitName= em.SplitName,
                                      amount = e.Amount,
                                      category = e.Category,
                                      dateTime = e.DateTime,
                                      lent = e.Lent,




                                  }).ToList();
                var managelistleft = (from e in context.Expenses
                                  join em in context.ManageExpenses on e.Expense_Id equals em.Expense_Id
                                  where e.Paidby != userId && em.User_Id != userId

                                  select new ManageVM
                                  {
                                      TotalBalance = context.ManageExpenses.Where(x => x.Expense_Id == e.Expense_Id && x.User_Id != userId).Select(x => x.Amount).Sum(),
                                      Paidby = e.Paidby,
                                      expenseId = e.Expense_Id,
                                      splitName = em.SplitName,
                                      amount = e.Amount,

                                  }).ToList();

                foreach (var item in managelist)
                {
                    item.PaidName = await GetUserName(item.Paidby);
                }



                return managelist;
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public async Task<List<ManageExpense>> GetAllManageExpenses()
        {
            try
            {
                AuthenticationState authState = await UserauthenticationStateProvider.GetAuthenticationStateAsync();
                ClaimsPrincipal user = authState.User;
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                //var manageData = (from exp in context.Expenses
                //                  join mexp in context.ManageExpenses on exp.Expense_Id equals mexp.Expense_Id
                //                  where mexp.User_Id == userId
                //                  select new ManageVM
                //                  {
                //                      SplittedName = mexp.SplitName,
                //                      TotalBalance = exp.Amount, //paidby Total Amount
                //                      OwedAmount = mexp.Amount,

                //                  }).ToList();
                var listofExpenses= await context.ManageExpenses.Where(x => x.User_Id != userId).ToListAsync();
                //var listofExpenses= await context.ManageExpenses.ToListAsync();
                foreach (var item in listofExpenses)
                {
                    item.SplitName = await GetUserName(item.User_Id);
                }



                return listofExpenses.OrderByDescending(x => x.Id).ToList();
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
        public async Task<DashboardVM> GetDasahboard()
        {
            DashboardVM dashboard = new();
            dashboard.Amount = context.Expenses.Count();
            return dashboard;

        }
        public async Task<string> GetUserName(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var username = (user == null ? "" : user.FirstName + " " + user.LastName);
            return username;
        }
    }
}
