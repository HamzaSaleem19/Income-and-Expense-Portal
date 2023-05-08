using Income_and_Expense.Data.Models;

namespace Income_and_Expense.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
