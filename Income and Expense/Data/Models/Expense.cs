using System;
using System.Collections.Generic;
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
        [NotMapped]
        public string[] UserIds { get; set; } = new string[] {  };
        public double Amount  { get; set; }
       
        public double Lent { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public int Group_Id { get; set; } //FK
        [ForeignKey("Group_Id")]
        public Groups Group { get; set; }
        public string Paidby { get; set; }
        [NotMapped]
        public string PaidName { get; set; }
        public string Equallysplited { get; set; }

    }
}
