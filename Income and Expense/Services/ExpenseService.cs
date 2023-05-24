using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Income_and_Expense.View_Models;
using Income_and_Expense.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Org.BouncyCastle.Pqc.Crypto.Frodo;
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

            var ManagePaidBy = await context.Expenses.Where(x => x.Paidby == expense.Paidby).ToListAsync();
            if (ManagePaidBy.Any())
            {
                foreach(var item in ManagePaidBy)
                {
                    var UpdatedPaidBy = await context.ManageExpenses.Where(x => x.Expense_Id == item.Expense_Id && x.Amount > 0 && x.User_Id != expense.Paidby).ToListAsync();
                    if (UpdatedPaidBy.Any())
                    {
                        foreach (var itemm in UpdatedPaidBy)
                        {
                            var checkAmount = from a in context.Expenses
                                              join m in context.ManageExpenses on a.Expense_Id equals m.Expense_Id
                                              where a.Paidby == itemm.User_Id && m.User_Id != itemm.User_Id
                                              select m;
                            foreach (var item1 in checkAmount)
                            {
                                item1.Amount = item1.Amount - (expense.Amount / expense.UserIds.Count());

                            }
                        }
                    }
                   
                }
            }
            else
            {

                var UpdatedPaidBy = await context.ManageExpenses.Where(x =>  x.Amount > 0 && x.User_Id == expense.Paidby).ToListAsync();
                if (UpdatedPaidBy.Any())
                {
                    foreach (var itemm in UpdatedPaidBy)
                    {
                        //var checkAmount = from a in context.Expenses
                        //                  join m in context.ManageExpenses on a.Expense_Id equals m.Expense_Id
                        //                  where  m.User_Id != itemm.User_Id
                        //                  select m;


                         
                           var checkPaidby = await context.Expenses.Where(x=> x.Expense_Id == itemm.Expense_Id).FirstOrDefaultAsync();

                           var managePaidby = await context.ManageExpenses.Where(x=> x.User_Id == checkPaidby.Paidby && x.User_Id != expense.Paidby).FirstOrDefaultAsync();
                        if(managePaidby.Amount - (expense.Amount / expense.UserIds.Count()) > 0)
                        {

                        }
                        else
                        {
                            
                        }
                        managePaidby.Amount = managePaidby.Amount - (expense.Amount / expense.UserIds.Count());

                         
                    }
                }
            }
           



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
                                  where e.Paidby == userId && em.User_Id == userId

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




                                  }).OrderByDescending(x => x.expenseId).ToList();
                //var managelistleft = (from e in context.Expenses
                //                      join em in context.ManageExpenses on e.Expense_Id equals em.Expense_Id
                //                      where e.Paidby != userId  
                //                      group new {e, em} by new {em.User_Id} into g
                //                      select new ManageVM
                //                      {
                //                         // TotalBalance = context.ManageExpenses.Where(x => x.Expense_Id == g.Select(x=>x.e.Expense_Id).FirstOrDefault() && x.User_Id != userId).Select(x => x.Amount).Sum(),
                //                          Paidby = g.Min(x=> x.e.Paidby),
                //                        // expenseId = g.Select(x=> x.e.Expense_Id).FirstOrDefault(),
                //                        // splitName =  g.Min(x=> ( x.em ==null ? "" : x.em.SplitName)),
                //                          amount = g.Sum(x=>x.e.Amount),

                //                      }).ToList();

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
        public async Task<ParentManageExpense> GetAllManageExpensesDashboard()
         {
            try
            {

                ParentManageExpense parentManageExpense =new ();
               AuthenticationState authState = await UserauthenticationStateProvider.GetAuthenticationStateAsync();
                ClaimsPrincipal user = authState.User;
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                // Payment given by me(Current User)
                parentManageExpense.listofExpenses = await (from exp in context.Expenses
                                  join mexp in context.ManageExpenses on exp.Expense_Id equals mexp.Expense_Id
                                  where mexp.User_Id != userId && exp.Paidby == userId
                                  group mexp by  mexp.User_Id  into g
                                  select new ManageExpense
                                  {
                                      User_Id = g.Key,
                                      Amount = g.Sum(x => x.Amount),
                                      Expense_Id = g.Min(x => x.Expense_Id)
                                  }).ToListAsync();

                // Payment return back(Other Users)
                parentManageExpense.PaidbyOthers = await (from exp in context.Expenses
                                  join mexp in context.ManageExpenses on exp.Expense_Id equals mexp.Expense_Id
                                  where mexp.User_Id != userId && exp.Paidby != userId && exp.Paidby == mexp.User_Id
                                  group mexp by  mexp.User_Id  into g
                                  select new ManageExpense
                                  {
                                      User_Id = g.Key,
                                      Amount = g.Sum(x => x.Amount),
                                      Expense_Id = g.Min(x => x.Expense_Id)
                                  }).ToListAsync();
                // var listofExpenses= await context.ManageExpenses.Where(x => x.User_Id != userId).ToListAsync();

               // var listofExpenses =await context.ManageExpenses.Where(x => x.User_Id != userId).GroupBy(x => x.User_Id).Select(
               //x => new ManageExpense()
               //{
               //    User_Id = x.Key,
               //    Amount = x.Sum(x => x.Amount),
               //    Expense_Id = x.Min(x => x.Expense_Id)
               //}).ToListAsync();
                //var listofExpenses= await context.ManageExpenses.ToListAsync();
                foreach (var item in parentManageExpense.listofExpenses)
                {
                    item.SplitName = await GetUserName(item.User_Id);
                }
                foreach (var item in parentManageExpense.PaidbyOthers)
                {
                    item.SplitName = await GetUserName(item.User_Id);
                }


                return parentManageExpense;
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

               // var listofExpenses = await context.ManageExpenses.Where(x => x.User_Id != userId).GroupBy(x => x.User_Id).Select(
               //x => new ManageExpense()
               //{
               //    User_Id = x.Key,
               //    Amount = x.Sum(x => x.Amount),
               //    Expense_Id = x.Min(x => x.Expense_Id)


               //}).ToListAsync();
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
