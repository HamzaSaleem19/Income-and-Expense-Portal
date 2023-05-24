using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Income_and_Expense.Apis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AddExpenseController : ControllerBase
    {
        public IEnumerable<string> valuess = new string[] { };
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment env;
        public AddExpenseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            this.context = context;
            _userManager = userManager;
            this.env = env;
        }

        [HttpGet]
        public async Task<IActionResult> TestAPI()
        {
            try
            {
                return Ok("Apis working fine");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        // Add Expense Api
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] Expense expense)
        {
            expense.Lent = Math.Round(expense.Amount - (expense.Amount / expense.UserIds.Count()));
            expense.UserIds = valuess.ToArray();
            await context.Expenses.AddAsync(expense);
            List<ManageExpense> ListOfmanageExpense = new();
            foreach (var item in expense.UserIds)
            {
                ManageExpense manageExpense = new ManageExpense();
                manageExpense.User_Id = item;
                manageExpense.Expense = expense;
                manageExpense.Amount = expense.Amount / expense.UserIds.Count();
                //expense.Lent = expense.Amount - manageExpense.Amount;
                ListOfmanageExpense.Add(manageExpense);
                //var lent = expense.Amount - manageExpense.Amount;
            }

            await context.ManageExpenses.AddRangeAsync(ListOfmanageExpense);
            await context.SaveChangesAsync();
            return Ok("Expense added Sucessfully!");
        }

    }

    }


