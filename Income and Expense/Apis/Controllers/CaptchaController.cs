using AspNetCore.ReCaptcha;
using Income_and_Expense.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Income_and_Expense.Apis.Controllers
{
    public class CaptchaController : Controller
    {

        [ValidateReCaptcha]
        [HttpPost]
        public IActionResult SubmitForm(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View("Index");
            TempData["Message"] = "Your form has been sent!";
            return RedirectToAction("Index");
        }
    }
}
