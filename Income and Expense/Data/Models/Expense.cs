using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Income_and_Expense.Data.Models
{
    public class Expense
    {
        [Key]
        public int Expense_Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Amount  { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public int Group_Id { get; set; } //FK
        [ForeignKey("Group_Id")]
        public Groups Group { get; set; }

    }
}
