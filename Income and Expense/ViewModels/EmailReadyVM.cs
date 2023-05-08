namespace Income_and_Expense.ViewModels
{
    public class EmailReadyVM
    {
        public string UserTitle { get; set; }
        public string ServiceName { get; set; }
        public string SelectedPackage { get; set; }
        public string EstimatePricing { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
        public int EmailTypeId { get; set; }
    }
}
