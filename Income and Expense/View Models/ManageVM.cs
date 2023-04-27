using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Income_and_Expense.View_Models
{
    public class ManageVM
    {
        public double TotalBalance { get; set; }
        public double OwedAmount { get; set; }
        public double Owe { get; set; }
        public string SplittedName { get; set; }
        public string Paidby { get; set; }
        public string PaidName { get; set; }
        public int expenseId { get; set; }
        public string splitName { get; set; }
        public double amount { get; set; }
        public string category { get; set; }
        public DateTime dateTime { get; set; }
        public double lent { get; set; }
        
    }
}
