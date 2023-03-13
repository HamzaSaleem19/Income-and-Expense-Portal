using Microsoft.AspNetCore.Identity;

namespace Income_and_Expense.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
    }
}
