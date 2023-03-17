using Income_and_Expense.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Income_and_Expense.Data.Models
{
    public class Groups
    {
        [Key]
        public int Group_Id { get; set; }
        public string Group_Name { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public GroupType Group_Type { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }



    }
}
