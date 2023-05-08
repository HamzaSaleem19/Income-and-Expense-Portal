using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Income_and_Expense.Services
{
    public class EmailblazorService
    {
        private readonly IEmailService _emailService;

        public EmailblazorService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // Method to Send Email

        public bool SendEmail(EmailDTO request)
        {
            _emailService.SendEmail(request);
            return true;

        }
    }
}
