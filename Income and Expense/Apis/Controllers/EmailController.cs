using Income_and_Expense.Data.Models;
using Income_and_Expense.Services;
using Microsoft.AspNetCore.Mvc;

namespace Income_and_Expense.Apis.Controllers
{
    public class EmailController : ControllerBase
    {

        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // Method to Send Email
        [HttpPost]
        public IActionResult SendEmail(EmailDTO request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
