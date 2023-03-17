using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Income_and_Expense.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }

    }
}
