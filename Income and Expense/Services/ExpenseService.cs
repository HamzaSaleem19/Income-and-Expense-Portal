﻿using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            //var userName = User.FindFirstValue(ClaimTypes.Name);
            var userslist = await userManager.Users.Select(x =>
                 new SelectListItem()
                 {
                     Value = x.Id,
                     Text = x.UserName
                 }).ToListAsync();
            return userslist;
        }
        public async Task<bool> AddExpense(Expense expense)
        {
            await context.Expenses.AddAsync(expense);
            return true;
        }
    }
}
