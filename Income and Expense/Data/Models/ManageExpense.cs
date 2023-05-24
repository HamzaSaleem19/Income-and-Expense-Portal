using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Income_and_Expense.Data.Models
{
    public class ManageExpense
    {
        [Key]
        public int Id { get; set; }
        public int Expense_Id { get; set; }
        [ForeignKey("Expense_Id")]
        public Expense Expense { get; set; }
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public ApplicationUser ApplicationUser { get; set; }

        public double Amount { get; set; }
        [NotMapped]
        public string SplitName { get; set; }
    }
    public class ParentManageExpense
    {
        public List<ManageExpense> listofExpenses { get; set; } = new();
        public List<ManageExpense> PaidbyOthers { get; set; }=new();
    }
    }
