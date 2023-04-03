using Income_and_Expense.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Income_and_Expense.Data.Models
{
    public class Groups
    {
        [NotMapped]
        public string[] UserIds { get; set; } = new string[] { };
        [Key]
        public int Group_Id { get; set; }
        public string Group_Name { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public GroupType Group_Type { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }

        public static implicit operator List<object>(Groups v)
        {
            throw new NotImplementedException();
        }
    }
}
