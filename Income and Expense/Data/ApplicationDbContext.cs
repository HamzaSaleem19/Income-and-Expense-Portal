using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Income_and_Expense.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Groups> Groupss { get; set; } 
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<ManageExpense> ManageExpenses { get; set; }

    }
}
